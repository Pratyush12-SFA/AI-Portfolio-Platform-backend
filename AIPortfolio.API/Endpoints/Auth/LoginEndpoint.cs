using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.Login;
using AIPortfolio.Application.Features.Auth.Queries.GetCurrentUser;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class LoginEndpoint
{
    public static async Task<IResult> PostLogin(
        LoginCommand command,
        ISender mediator,
        ICookieService cookieService,
        HttpContext httpContext)
    {
        var result = await mediator.Send(command);
        if (result.IsSuccess)
            cookieService.SetRefreshTokenCookie(
                httpContext.Response,
                result.Value.RefreshToken,
                command.RememberMe);
        return result.ToApiResult();
    }

    public static async Task<IResult> GetMe(ISender mediator)
    {
        var result = await mediator.Send(new GetCurrentUserQuery());
        return result.ToApiResult();
    }
}