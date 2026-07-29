using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IChatService
{
    Task<AIChatSession> CreateSessionAsync(long userId, string title);
    Task<IEnumerable<AIChatSession>> GetUserSessionsAsync(long userId);
    Task<IEnumerable<AIChatMessage>> GetSessionMessagesAsync(long sessionId);
    Task<AIChatMessage> SendMessageAsync(long sessionId, string userContent);
}