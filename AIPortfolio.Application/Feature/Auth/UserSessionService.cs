using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class UserSessionService
    : IUserSessionService
{
    private readonly IUserSessionRepository
        _userSessionRepository;

    private readonly IUserInfoAccessor
        _userInfoAccessor;

    public UserSessionService(
        IUserSessionRepository userSessionRepository,
        IUserInfoAccessor userInfoAccessor)
    {
        _userSessionRepository =
            userSessionRepository;

        _userInfoAccessor =
            userInfoAccessor;
    }

    public async Task<IEnumerable<UserSessionResponse>>
        GetSessionsAsync()
    {
        long userId =
            _userInfoAccessor.UserId;

        var sessions =
            await _userSessionRepository
                .GetUserSessionsAsync(
                    userId);

        return sessions.Select(
            x => new UserSessionResponse
            {
                Id = x.Id,

                DeviceName =
                    x.UserAgent,

                CreatedFromIp =
                    x.CreatedFromIp,

                LoginAt =
                    x.CreatedOn,
                
                IsCurrentSession =
                    false
            });
    }
}