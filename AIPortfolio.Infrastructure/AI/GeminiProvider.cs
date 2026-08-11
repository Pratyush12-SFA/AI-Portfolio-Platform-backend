using System.Text;
using System.Text.Json;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace AIPortfolio.Infrastructure.AI;

public sealed class GeminiProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;
    private readonly IAIUsageService _usageService;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public GeminiProvider(
        HttpClient httpClient,
        IOptions<GeminiSettings>? settings,
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
        var model = string.IsNullOrWhiteSpace(modelName) ? _settings.DefaultModel : modelName;

        var apiKey = _settings.GeminiApiKey;
        if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "PLACEHOLDER_KEY")
            apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? apiKey;

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "PLACEHOLDER_KEY")
            throw new InvalidOperationException(
                "Gemini API Key is not configured. Please set a valid key in 'GeminiSettings:GeminiApiKey' inside appsettings.json or define the 'GEMINI_API_KEY' system environment variable.");

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

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
        var status = "Success";
        var inputTokens = 0;
        var outputTokens = 0;
        var generatedText = string.Empty;

        try
        {
            var response = await _httpClient.PostAsync(url, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                status = $"Error: {response.StatusCode}";
                var errContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API returned error: {response.StatusCode} - {errContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
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
                    generatedText = partsProp[0].GetProperty("text").GetString() ?? string.Empty;
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
            var userId = _userInfoAccessor.IsAuthenticated ? _userInfoAccessor.UserId : 0;

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
        var model = string.IsNullOrWhiteSpace(modelName) ? _settings.DefaultModel : modelName;
        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:streamGenerateContent?key={_settings.GeminiApiKey}";

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

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        // Parse line-by-line for server-sent stream segments
        while (reader is not { EndOfStream: true }) 
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            // Trim out "data: " prefix if present or parse raw JSON chunks
            var jsonChunk = line.Trim();
            if (jsonChunk.StartsWith("[")) jsonChunk = jsonChunk.TrimStart('[');
            if (jsonChunk.EndsWith("]")) jsonChunk = jsonChunk.TrimEnd(']');
            if (jsonChunk.EndsWith(",")) jsonChunk = jsonChunk.TrimEnd(',');

            var textPart = string.Empty;
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
                        textPart = partsProp[0].GetProperty("text").GetString() ?? string.Empty;
                }
            }
            catch
            {
                // Ignore incomplete JSON chunks during streaming parses
            }

            if (!string.IsNullOrEmpty(textPart)) yield return textPart;
        }
    }
}