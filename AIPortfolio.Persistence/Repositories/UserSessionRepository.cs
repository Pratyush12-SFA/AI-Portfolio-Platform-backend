using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class UserSessionRepository : IUserSessionRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public UserSessionRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateSessionAsync(UserSession userSession)
    {
        _dbContext.UserSessions.Add(userSession);
        await _dbContext.SaveChangesAsync();
        return userSession.Id;
    }

    public async Task RevokeSessionAsync(long refreshTokenId, string revokedBy)
    {
        var session = await _dbContext.UserSessions.FirstOrDefaultAsync(s => s.RefreshTokenId == refreshTokenId);
        if (session != null)
        {
            session.IsActive = false;
            session.DeletedOn = DateTime.UtcNow;
            session.DeletedBy = revokedBy;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<UserSession>> GetActiveSessionsAsync(long userId)
    {
        return await _dbContext.UserSessions
            .Where(s => s.UserId == userId && s.IsActive && !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserSession>> GetUserSessionsAsync(long userId)
    {
        return await _dbContext.UserSessions
            .Where(s => s.UserId == userId && !s.IsDeleted)
            .ToListAsync();
    }
}