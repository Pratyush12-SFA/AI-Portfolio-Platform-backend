using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Abstractions;

public interface IUserSessionService
{
    Task<IEnumerable<UserSessionResponse>>
        GetSessionsAsync();
}