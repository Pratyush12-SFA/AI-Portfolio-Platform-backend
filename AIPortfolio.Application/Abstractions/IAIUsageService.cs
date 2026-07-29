namespace AIPortfolio.Application.Abstractions;

public interface IAIUsageService
{
    Task LogUsageAsync(
        long userId,
        string feature,
        string model,
        string promptType,
        int inputTokens,
        int outputTokens,
        int durationMs,
        string status);
}