using System;

namespace AIPortfolio.Infrastructure.AI;

public static class TokenCounter
{
    // Approximately 4 characters per token as a standard industry approximation rule
    public static int EstimateTokens(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        return (int)Math.Ceiling(text.Length / 4.0);
    }

    public static decimal CalculateCost(string model, int inputTokens, int outputTokens)
    {
        decimal inputRate = 0m;
        decimal outputRate = 0m;

        // Realistic pricing model per 1M tokens
        if (model.Contains("pro", StringComparison.OrdinalIgnoreCase))
        {
            // gemini-2.5-pro pricing
            inputRate = 1.25m / 1_000_000m;
            outputRate = 5.00m / 1_000_000m;
        }
        else
        {
            // gemini-2.5-flash (default model rate)
            inputRate = 0.075m / 1_000_000m;
            outputRate = 0.30m / 1_000_000m;
        }

        return (inputTokens * inputRate) + (outputTokens * outputRate);
    }
}
