using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IUserSessionRepository
{
    Task<long> CreateSessionAsync(
        UserSession userSession);

    Task RevokeSessionAsync(
        long refreshTokenId,
        string revokedBy);

    Task<IEnumerable<UserSession>>
        GetActiveSessionsAsync(
            long userId);
    
    Task<IEnumerable<UserSession>>
        GetUserSessionsAsync(
            long userId);
}