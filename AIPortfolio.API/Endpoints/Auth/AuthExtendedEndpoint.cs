using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Features.Auth.Commands.ChangePassword;
using AIPortfolio.Application.Features.Auth.Commands.ForgotPassword;
using AIPortfolio.Application.Features.Auth.Commands.ResetPassword;
using AIPortfolio.Application.Features.Auth.Commands.VerifyEmail;
using AIPortfolio.Application.Features.Auth.Queries.GetActiveSessions;
using AIPortfolio.Application.Features.Auth.Queries.RevokeSession;
using MediatR;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class AuthExtendedEndpoint
{
    public static async Task<IResult> ForgotPassword(
        ForgotPasswordCommand command,
        ISender mediator)
    {
        await mediator.Send(command);
        return Results.Ok(new
            { success = true, message = "If the email exists, a password reset link has been sent." });
    }

    public static async Task<IResult> ResetPassword(
        ResetPasswordCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> SendVerification(
        IUserInfoAccessor userInfoAccessor,
        IAuthService authService)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        await authService.SendVerificationEmailAsync(userInfoAccessor.Email);
        return Results.Ok(new { success = true, message = "Verification email has been sent." });
    }

    public static async Task<IResult> VerifyEmail(
        VerifyEmailCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> ChangePassword(
        ChangePasswordCommand command,
        ISender mediator,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        var result = await mediator.Send(command with { UserId = userInfoAccessor.UserId });
        return result.ToApiResult();
    }

    public static async Task<IResult> GetActiveSessions(
        HttpContext httpContext,
        ISender mediator,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        string? refreshToken = null;
        if (httpContext.Request.Cookies.TryGetValue("refresh_token", out var tokenValue) &&
            !string.IsNullOrEmpty(tokenValue)) refreshToken = tokenValue;

        var result = await mediator.Send(new GetActiveSessionsQuery(userInfoAccessor.UserId, refreshToken));
        return result.ToApiResult();
    }

    public static async Task<IResult> RevokeSession(
        long refreshTokenId,
        ISender mediator,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated) return Results.Unauthorized();

        var result = await mediator.Send(new RevokeSessionCommand(refreshTokenId, userInfoAccessor.Email));
        return result.ToApiResult();
    }
}