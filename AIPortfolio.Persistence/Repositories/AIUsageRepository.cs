using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using System;
using System.Threading.Tasks;

namespace AIPortfolio.Persistence.Repositories;

public sealed class AIUsageRepository : IAIUsageRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public AIUsageRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> CreateAsync(AIUsage usage)
    {
        usage.CreatedOn = DateTime.UtcNow;
        _dbContext.AIUsages.Add(usage);
        await _dbContext.SaveChangesAsync();
        return usage.Id;
    }
}
