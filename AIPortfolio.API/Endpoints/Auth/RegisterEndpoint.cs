using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.Register;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class RegisterEndpoint
{
    public static async Task<IResult> PostRegister(
        RegisterCommand command,
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