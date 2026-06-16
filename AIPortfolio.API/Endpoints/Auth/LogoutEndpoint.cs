using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class LogoutEndpoint
{
    public static async Task<IResult>
        PostLogout(
            LogoutRequest request,
            ILogoutService logoutService)
    {
        await logoutService
            .LogoutAsync(request);

        return Results.Ok();
    }
}