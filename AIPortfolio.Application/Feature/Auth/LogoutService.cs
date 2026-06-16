using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Feature.Auth;

public sealed class LogoutService
    : ILogoutService
{
    private readonly IRefreshTokenRepository
        _refreshTokenRepository;

    public LogoutService(
        IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository =
            refreshTokenRepository;
    }

    public async Task LogoutAsync(
        LogoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(
                request.RefreshToken))
        {
            return;
        }
        

        await _refreshTokenRepository
            .RemoveAsync(
                request.RefreshToken,
                "System");
    }
}