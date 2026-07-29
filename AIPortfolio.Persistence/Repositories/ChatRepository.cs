using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIPortfolio.Persistence.Repositories;

public sealed class ChatRepository : IChatRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public ChatRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<AIChatSession>> ListSessionsAsync(long userId)
    {
        return await _dbContext.AIChatSessions
            .Where(s => s.UserId == userId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedOn)
            .ToListAsync();
    }

    public async Task<AIChatSession?> GetSessionByIdAsync(long sessionId)
    {
        return await _dbContext.AIChatSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && !s.IsDeleted);
    }

    public async Task<long> CreateSessionAsync(AIChatSession session)
    {
        session.CreatedOn = DateTime.UtcNow;
        _dbContext.AIChatSessions.Add(session);
        await _dbContext.SaveChangesAsync();
        return session.Id;
    }

    public async Task<IEnumerable<AIChatMessage>> ListMessagesAsync(long sessionId)
    {
        return await _dbContext.AIChatMessages
            .Where(m => m.SessionId == sessionId && !m.IsDeleted)
            .OrderBy(m => m.CreatedOn)
            .ToListAsync();
    }

    public async Task<long> CreateMessageAsync(AIChatMessage message)
    {
        message.CreatedOn = DateTime.UtcNow;
        _dbContext.AIChatMessages.Add(message);
        await _dbContext.SaveChangesAsync();
        return message.Id;
    }
}
