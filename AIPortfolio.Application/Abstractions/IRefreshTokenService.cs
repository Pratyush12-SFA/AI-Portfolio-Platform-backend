using AIPortfolio.Application.DTOs.Auth;


namespace AIPortfolio.Application.Abstractions;

public interface IRefreshTokenService
{
    Task<RefreshTokenResponse?> RefreshAsync(
        RefreshTokenRequest refreshTokenRequest);
}