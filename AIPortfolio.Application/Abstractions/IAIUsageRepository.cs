using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IAIUsageRepository
{
    Task<long> CreateAsync(AIUsage usage);
}