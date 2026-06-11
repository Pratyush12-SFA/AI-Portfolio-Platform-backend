using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class GoogleLoginEndpoint
{
    public static async Task<IResult> PostGoogleLogin(
        GoogleLoginRequest request,
        IGoogleAuthService googleAuthService)
    {
        LoginResponse? response =
            await googleAuthService.LoginAsync(request);

        if (response is null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(response);
    }
}