using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.RefreshToken;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class RefreshTokenEndpoint
{
    public static async Task<IResult> PostRefreshToken(
        HttpContext httpContext,
        ICookieService cookieService,
        ISender mediator)
    {
        var refreshToken = cookieService.GetRefreshTokenCookie(httpContext.Request);
        if (string.IsNullOrWhiteSpace(refreshToken)) return Results.Unauthorized();

        var result = await mediator.Send(new RefreshTokenCommand(refreshToken));
        if (result.IsSuccess)
            cookieService.SetRefreshTokenCookie(
                httpContext.Response,
                result.Value.RefreshToken,
                true);

        return result.ToApiResult();
    }
}