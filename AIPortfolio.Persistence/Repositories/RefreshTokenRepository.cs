using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public RefreshTokenRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
        return refreshToken.Id;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token && !r.IsRevoked && r.ExpiresAt > DateTime.UtcNow);
    }

    public async Task RemoveAsync(string token, string revokedBy)
    {
        var rt = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
        if (rt != null)
        {
            rt.IsRevoked = true;
            rt.RevokedAt = DateTime.UtcNow;
            rt.RevokedBy = revokedBy;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RevokeAllAsync(long userId, string revokedBy)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedBy = revokedBy;
        }

        await _dbContext.SaveChangesAsync();
    }
}