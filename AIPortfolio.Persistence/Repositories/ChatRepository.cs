using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AIPortfolio.Persistence.Repositories;

public sealed class ChatRepository : IChatRepository
{
    private readonly DapperContext _context;

    public ChatRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<AIChatSession>> ListSessionsAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<AIChatSession>(
            "Portfolio.usp_AIChatSession_List",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<AIChatSession?> GetSessionByIdAsync(long sessionId)
    {
        using IDbConnection connection = _context.CreateConnection();
        string sql = "SELECT Id, UserId, Title, CreatedOn, UpdatedOn FROM Portfolio.AIChatSessions WHERE Id = @SessionId";
        return await connection.QueryFirstOrDefaultAsync<AIChatSession>(sql, new { SessionId = sessionId });
    }

    public async Task<long> CreateSessionAsync(AIChatSession session)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(
            "Portfolio.usp_AIChatSession_Create",
            new { UserId = session.UserId, Title = session.Title },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<AIChatMessage>> ListMessagesAsync(long sessionId)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<AIChatMessage>(
            "Portfolio.usp_AIChatMessage_List",
            new { SessionId = sessionId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> CreateMessageAsync(AIChatMessage message)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(
            "Portfolio.usp_AIChatMessage_Create",
            new
            {
                SessionId = message.SessionId,
                Role = message.Role,
                Content = message.Content,
                InputTokens = message.InputTokens,
                OutputTokens = message.OutputTokens
            },
            commandType: CommandType.StoredProcedure);
    }
}
