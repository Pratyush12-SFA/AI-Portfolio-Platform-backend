using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Repositories;

public sealed class PromptRepository : IPromptRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public PromptRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<PromptTemplate?> GetActiveTemplateByFeatureAsync(string feature)
    {
        return await _dbContext.PromptTemplates
            .FirstOrDefaultAsync(p => p.Feature == feature && p.IsActive && !p.IsDeleted);
    }

    public async Task<long> CreateTemplateAsync(PromptTemplate template)
    {
        template.CreatedOn = DateTime.UtcNow;
        _dbContext.PromptTemplates.Add(template);
        await _dbContext.SaveChangesAsync();
        return template.Id;
    }
}