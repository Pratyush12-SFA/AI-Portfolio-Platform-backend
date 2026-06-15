using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class RefreshTokenEndpoint
{
    public static async Task<IResult> PostRefreshToken(
        RefreshTokenRequest request,
        IRefreshTokenService refreshTokenService)
    {
        RefreshTokenResponse? response =
            await refreshTokenService.RefreshAsync(
                request);
        if (response is null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(response);
    }
}