using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.GoogleLogin;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class GoogleLoginEndpoint
{
    public static async Task<IResult> PostGoogleLogin(
        GoogleLoginCommand command,
        ISender mediator,
        ICookieService cookieService,
        HttpContext httpContext)
    {
        var result = await mediator.Send(command);
        if (result.IsSuccess)
            cookieService.SetRefreshTokenCookie(
                httpContext.Response,
                result.Value.RefreshToken,
                true);
        return result.ToApiResult();
    }
}