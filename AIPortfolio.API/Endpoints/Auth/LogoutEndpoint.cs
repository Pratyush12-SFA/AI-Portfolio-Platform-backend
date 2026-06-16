using AIPortfolio.Application.Abstractions;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class LogoutEndpoint
{
    public static async Task<IResult>
        PostLogout(
            ICookieService cookieService,
            HttpContext httpContext,
           IRefreshTokenRepository refreshTokenRepository)
    {
        string? refreshToken = cookieService.GetRefreshTokenCookie(
            httpContext.Request);
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            await refreshTokenRepository.RemoveAsync(refreshToken,
                "SYSTEM");
        }
        
        cookieService.DeleteRefreshTokenCookie(
            httpContext.Response);
       

        return Results.Ok();
    }
}