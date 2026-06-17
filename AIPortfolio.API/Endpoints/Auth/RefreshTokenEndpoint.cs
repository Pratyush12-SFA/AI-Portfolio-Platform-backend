using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class RefreshTokenEndpoint
{
    public static async Task<IResult> PostRefreshToken(
        HttpContext httpContext,
        ICookieService cookieService,
        IRefreshTokenService refreshTokenService)
    {
        
        string? refreshToken =
            cookieService.GetRefreshTokenCookie(
                httpContext.Request);

        if (string.IsNullOrWhiteSpace(
                refreshToken))
        {
            return Results.Unauthorized();
        }

        RefreshTokenResponse? response =
            await refreshTokenService.RefreshAsync(
                new RefreshTokenRequest
                {
                    RefreshToken = refreshToken
                });

        if (response is null)
        {
            return Results.Unauthorized();
        }

        cookieService.SetRefreshTokenCookie(
            httpContext.Response,
            response.RefreshToken,
            true);

        return Results.Ok(
            new
            {
                response.AccessToken
            });
        
    }
}