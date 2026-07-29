using System.Net.Sockets;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AIPortfolio.Infrastructure.AI;

public sealed class SmartAIProvider : IAIProvider
{
    private readonly DevelopmentAIProvider _devProvider;
    private readonly GeminiProvider _geminiProvider;
    private readonly ILogger<SmartAIProvider> _logger;
    private readonly GeminiSettings _settings;

    public SmartAIProvider(
        GeminiProvider geminiProvider,
        DevelopmentAIProvider devProvider,
        IOptions<GeminiSettings> settings,
        ILogger<SmartAIProvider> logger)
    {
        _geminiProvider = geminiProvider ?? throw new ArgumentNullException(nameof(geminiProvider));
        _devProvider = devProvider ?? throw new ArgumentNullException(nameof(devProvider));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        string modelName,
        bool requestJson = false)
    {
        var useGemini = !string.IsNullOrWhiteSpace(_settings.GeminiApiKey) &&
                        _settings.GeminiApiKey != "PLACEHOLDER_KEY";

        if (useGemini)
            try
            {
                return await _geminiProvider.GenerateAsync(systemPrompt, userPrompt, modelName, requestJson);
            }
            catch (Exception ex) when (IsTransientOrApiError(ex))
            {
                _logger.LogWarning(ex,
                    "Gemini API call failed due to network/external issue. Falling back to Development AI Provider.");
            }
        else
            _logger.LogInformation("Gemini API key is not configured or placeholder. Using Development AI Provider.");

        return await _devProvider.GenerateAsync(systemPrompt, userPrompt, modelName, requestJson);
    }

    public async IAsyncEnumerable<string> StreamAsync(
        string systemPrompt,
        string userPrompt,
        string modelName)
    {
        var useGemini = !string.IsNullOrWhiteSpace(_settings.GeminiApiKey) &&
                        _settings.GeminiApiKey != "PLACEHOLDER_KEY";

        var failed = false;

        if (useGemini)
        {
            IAsyncEnumerator<string>? enumerator = null;
            try
            {
                enumerator = _geminiProvider.StreamAsync(systemPrompt, userPrompt, modelName).GetAsyncEnumerator();
            }
            catch (Exception ex) when (IsTransientOrApiError(ex))
            {
                _logger.LogWarning(ex, "Gemini API stream initiation failed. Falling back to Development AI Provider.");
                failed = true;
            }

            if (!failed && enumerator != null)
                while (true)
                {
                    string? item = null;
                    try
                    {
                        if (!await enumerator.MoveNextAsync()) break;
                        item = enumerator.Current;
                    }
                    catch (Exception ex) when (IsTransientOrApiError(ex))
                    {
                        _logger.LogWarning(ex,
                            "Gemini API stream failed mid-transit. Falling back to Development AI Provider.");
                        failed = true;
                        break;
                    }

                    if (item != null) yield return item;
                }
        }

        if (!useGemini || failed)
            await foreach (var item in _devProvider.StreamAsync(systemPrompt, userPrompt, modelName))
                yield return item;
    }

    private static bool IsTransientOrApiError(Exception ex)
    {
        // Recursively inspect the exception hierarchy
        var current = ex;
        while (current != null)
        {
            if (current is HttpRequestException ||
                current is IOException ||
                current is SocketException ||
                current is TimeoutException ||
                current is TaskCanceledException)
                return true;
            current = current.InnerException;
        }

        return false;
    }
}