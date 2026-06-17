using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class GoogleLoginEndpoint
{
    public static async Task<IResult> PostGoogleLogin(
        GoogleLoginRequest request,
        IGoogleAuthService googleAuthService,
        ICookieService cookieService,
        HttpContext httpContext)
    {
        LoginResponse? response =
            await googleAuthService.LoginAsync(request);

        if (response is null)
        {
            return Results.Unauthorized();
        }
        cookieService.SetRefreshTokenCookie(
            httpContext.Response,
            response.RefreshToken,
            true);
        return Results.Ok(new
        {
            response.AccessToken,
            response.Email,
            response.FullName,
        });
    }
}