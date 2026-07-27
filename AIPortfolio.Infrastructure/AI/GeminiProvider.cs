using AIPortfolio.Application.Abstractions;
using AIPortfolio.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIPortfolio.Infrastructure.AI;

public sealed class GeminiProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;
    private readonly IUserInfoAccessor _userInfoAccessor;
    private readonly IAIUsageService _usageService;

    public GeminiProvider(
        HttpClient httpClient,
        IOptions<GeminiSettings> settings,
        IUserInfoAccessor userInfoAccessor,
        IAIUsageService usageService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _userInfoAccessor = userInfoAccessor ?? throw new ArgumentNullException(nameof(userInfoAccessor));
        _usageService = usageService ?? throw new ArgumentNullException(nameof(usageService));
    }

    public async Task<string> GenerateAsync(
        string systemPrompt, 
        string userPrompt, 
        string modelName, 
        bool requestJson = false)
    {
        string model = string.IsNullOrWhiteSpace(modelName) ? _settings.DefaultModel : modelName;
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_settings.GeminiApiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = userPrompt }
                    }
                }
            },
            systemInstruction = new
            {
                parts = new[]
                {
                    new { text = systemPrompt }
                }
            },
            generationConfig = requestJson ? new { responseMimeType = "application/json" } : null
        };

        var requestContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var startTime = DateTime.UtcNow;
        string status = "Success";
        int inputTokens = 0;
        int outputTokens = 0;
        string generatedText = string.Empty;

        try
        {
            var response = await _httpClient.PostAsync(url, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                status = $"Error: {response.StatusCode}";
                string errContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API returned error: {response.StatusCode} - {errContent}");
            }

            string responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            
            // Extract usage metadata if present
            if (doc.RootElement.TryGetProperty("usageMetadata", out var usageProp))
            {
                if (usageProp.TryGetProperty("promptTokenCount", out var promptProp))
                    inputTokens = promptProp.GetInt32();
                if (usageProp.TryGetProperty("candidatesTokenCount", out var candProp))
                    outputTokens = candProp.GetInt32();
            }

            // Extract content text
            if (doc.RootElement.TryGetProperty("candidates", out var candidatesProp) && 
                candidatesProp.GetArrayLength() > 0)
            {
                var candidate = candidatesProp[0];
                if (candidate.TryGetProperty("content", out var contentProp) && 
                    contentProp.TryGetProperty("parts", out var partsProp) && 
                    partsProp.GetArrayLength() > 0)
                {
                    generatedText = partsProp[0].GetProperty("text").GetString() ?? string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            status = $"Failed: {ex.Message}";
            throw;
        }
        finally
        {
            var duration = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
            long userId = _userInfoAccessor.IsAuthenticated ? _userInfoAccessor.UserId : 0;
            
            // If tokens are zero, estimate using length rules (approx. 4 characters = 1 token)
            if (inputTokens == 0) inputTokens = userPrompt.Length / 4 + systemPrompt.Length / 4;
            if (outputTokens == 0) outputTokens = generatedText.Length / 4;

            await _usageService.LogUsageAsync(
                userId, 
                "Generate", 
                model, 
                requestJson ? "JSON" : "Text", 
                inputTokens, 
                outputTokens, 
                duration, 
                status);
        }

        return generatedText;
    }

    public async IAsyncEnumerable<string> StreamAsync(
        string systemPrompt, 
        string userPrompt, 
        string modelName)
    {
        string model = string.IsNullOrWhiteSpace(modelName) ? _settings.DefaultModel : modelName;
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:streamGenerateContent?key={_settings.GeminiApiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = userPrompt }
                    }
                }
            },
            systemInstruction = new
            {
                parts = new[]
                {
                    new { text = systemPrompt }
                }
            }
        };

        var requestContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = requestContent
        };

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        // Parse line-by-line for server-sent stream segments
        while (!reader.EndOfStream)
        {
            string? line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            // Trim out "data: " prefix if present or parse raw json chunks
            string jsonChunk = line.Trim();
            if (jsonChunk.StartsWith("[")) jsonChunk = jsonChunk.TrimStart('[');
            if (jsonChunk.EndsWith("]")) jsonChunk = jsonChunk.TrimEnd(']');
            if (jsonChunk.EndsWith(",")) jsonChunk = jsonChunk.TrimEnd(',');

            string textPart = string.Empty;
            try
            {
                using var doc = JsonDocument.Parse(jsonChunk);
                if (doc.RootElement.TryGetProperty("candidates", out var candidatesProp) && 
                    candidatesProp.GetArrayLength() > 0)
                {
                    var candidate = candidatesProp[0];
                    if (candidate.TryGetProperty("content", out var contentProp) && 
                        contentProp.TryGetProperty("parts", out var partsProp) && 
                        partsProp.GetArrayLength() > 0)
                    {
                        textPart = partsProp[0].GetProperty("text").GetString() ?? string.Empty;
                    }
                }
            }
            catch
            {
                // Ignore incomplete json chunks during streaming parses
            }

            if (!string.IsNullOrEmpty(textPart))
            {
                yield return textPart;
            }
        }
    }
}
