using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IChatRepository
{
    Task<IEnumerable<AIChatSession>> ListSessionsAsync(long userId);
    Task<AIChatSession?> GetSessionByIdAsync(long sessionId);
    Task<long> CreateSessionAsync(AIChatSession session);
    Task<IEnumerable<AIChatMessage>> ListMessagesAsync(long sessionId);
    Task<long> CreateMessageAsync(AIChatMessage message);
}