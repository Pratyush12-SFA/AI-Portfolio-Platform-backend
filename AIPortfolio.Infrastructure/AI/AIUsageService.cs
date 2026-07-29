using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Infrastructure.AI;

public sealed class AIUsageService : IAIUsageService
{
    private readonly IAIUsageRepository _usageRepository;

    public AIUsageService(IAIUsageRepository usageRepository)
    {
        _usageRepository = usageRepository ?? throw new ArgumentNullException(nameof(usageRepository));
    }

    public async Task LogUsageAsync(
        long userId,
        string feature,
        string model,
        string promptType,
        int inputTokens,
        int outputTokens,
        int durationMs,
        string status)
    {
        var estimatedCost = TokenCounter.CalculateCost(model, inputTokens, outputTokens);

        var usage = new AIUsage
        {
            UserId = userId,
            Feature = feature,
            Model = model,
            PromptType = promptType,
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            EstimatedCost = estimatedCost,
            DurationMs = durationMs,
            Status = status,
            CreatedOn = DateTime.UtcNow
        };

        await _usageRepository.CreateAsync(usage);
    }
}