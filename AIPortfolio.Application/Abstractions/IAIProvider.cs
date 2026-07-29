namespace AIPortfolio.Application.Abstractions;

public interface IAIProvider
{
    Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        string modelName,
        bool requestJson = false);

    IAsyncEnumerable<string> StreamAsync(
        string systemPrompt,
        string userPrompt,
        string modelName);
}