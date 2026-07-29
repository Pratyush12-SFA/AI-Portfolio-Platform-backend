using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.Logout;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class LogoutEndpoint
{
    public static async Task<IResult> PostLogout(
        ICookieService cookieService,
        HttpContext httpContext,
        ISender mediator)
    {
        var refreshToken = cookieService.GetRefreshTokenCookie(httpContext.Request);
        var result = await mediator.Send(new LogoutCommand(refreshToken));

        cookieService.DeleteRefreshTokenCookie(httpContext.Response);

        return result.ToApiResult();
    }
}