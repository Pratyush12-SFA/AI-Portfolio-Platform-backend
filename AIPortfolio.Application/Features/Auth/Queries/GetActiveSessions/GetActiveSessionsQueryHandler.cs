using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Auth.Queries.GetActiveSessions;

internal sealed class
    GetActiveSessionsQueryHandler : IRequestHandler<GetActiveSessionsQuery, Result<IEnumerable<UserSessionResponse>>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public GetActiveSessionsQueryHandler(
        IUserSessionRepository sessionRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUserInfoAccessor userInfoAccessor)
    {
        _sessionRepository = sessionRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _userInfoAccessor = userInfoAccessor;
    }

    public async Task<Result<IEnumerable<UserSessionResponse>>> Handle(GetActiveSessionsQuery query,
        CancellationToken cancellationToken)
    {
        var sessions = (await _sessionRepository.GetActiveSessionsAsync(query.UserId)).ToList();

        if (!string.IsNullOrEmpty(query.CurrentRefreshToken))
        {
            var tokenRecord = await _refreshTokenRepository.GetByTokenAsync(query.CurrentRefreshToken);
            if (tokenRecord != null)
            {
                var currentSession = sessions.FirstOrDefault(s => s.RefreshTokenId == tokenRecord.Id);
                if (currentSession != null) currentSession.IsCurrentActive = true;
            }
        }

        if (!sessions.Any(s => s.IsCurrentActive))
        {
            var ip = _userInfoAccessor.GetRemoteIp();
            var ua = _userInfoAccessor.GetUserAgent();
            var match = sessions.FirstOrDefault(s => s.IpAddress == ip && s.UserAgent == ua)
                        ?? sessions.OrderByDescending(s => s.LastActiveAt).FirstOrDefault();
            if (match != null) match.IsCurrentActive = true;
        }

        var result = sessions.Select(x => new UserSessionResponse
        {
            Id = x.Id,
            DeviceName = x.UserAgent,
            CreatedFromIp = x.CreatedFromIp,
            LoginAt = x.CreatedOn,
            IsCurrentSession = x.IsCurrentActive
        });

        return Result.Success(result);
    }
}