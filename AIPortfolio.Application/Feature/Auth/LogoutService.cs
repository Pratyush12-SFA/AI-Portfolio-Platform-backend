using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class LogoutService
    : ILogoutService
{
    private readonly IRefreshTokenRepository
        _refreshTokenRepository;
    private readonly IUserSessionRepository _userSessionRepository;

    public LogoutService(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository userSessionRepository)
    {
        _refreshTokenRepository =
            refreshTokenRepository;
        _userSessionRepository = userSessionRepository;
    }

    public async Task LogoutAsync(
        LogoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(
                request.RefreshToken))
        {
            return;
        }

        RefreshToken? refreshToken =
            await _refreshTokenRepository
                .GetByTokenAsync(
                    request.RefreshToken);

        if (refreshToken is null)
        {
            return;
        }

        await _userSessionRepository
            .RevokeSessionAsync(
                refreshToken.Id,
                "System");

        await _refreshTokenRepository
            .RemoveAsync(
                request.RefreshToken,
                "System");
    }
}