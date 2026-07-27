using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class AuthExtendedEndpoint
{
    public static async Task<IResult> ForgotPassword(
        ForgotPasswordRequest request,
        IAuthService authService)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Results.BadRequest(new { message = "Email is required" });
        }

        bool success = await authService.ForgotPasswordAsync(request.Email);
        // Always return success/Ok to prevent user enumeration
        return Results.Ok(new { success = true, message = "If the email exists, a password reset link has been sent." });
    }

    public static async Task<IResult> ResetPassword(
        ResetPasswordRequest request,
        IAuthService authService)
    {
        if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return Results.BadRequest(new { message = "Token and password are required" });
        }

        bool success = await authService.ResetPasswordAsync(request.Token, request.NewPassword);
        if (!success)
        {
            return Results.BadRequest(new { message = "Invalid or expired reset token." });
        }

        return Results.Ok(new { success = true, message = "Password has been reset successfully." });
    }

    public static async Task<IResult> SendVerification(
        IAuthService authService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        bool success = await authService.SendVerificationEmailAsync(userInfoAccessor.Email);
        return Results.Ok(new { success = true, message = "Verification email has been sent." });
    }

    public static async Task<IResult> VerifyEmail(
        VerifyEmailRequest request,
        IAuthService authService)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return Results.BadRequest(new { message = "Token is required" });
        }

        bool success = await authService.VerifyEmailAsync(request.Token);
        if (!success)
        {
            return Results.BadRequest(new { message = "Invalid or expired verification token." });
        }

        return Results.Ok(new { success = true, message = "Email verified successfully." });
    }

    public static async Task<IResult> ChangePassword(
        ChangePasswordRequest request,
        IAuthService authService,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return Results.BadRequest(new { message = "Old password and new password are required" });
        }

        bool success = await authService.ChangePasswordAsync(userInfoAccessor.UserId, request.OldPassword, request.NewPassword);
        if (!success)
        {
            return Results.BadRequest(new { message = "Incorrect old password." });
        }

        return Results.Ok(new { success = true, message = "Password updated successfully." });
    }

    public static async Task<IResult> GetActiveSessions(
        IUserSessionRepository sessionRepository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        var sessions = await sessionRepository.GetActiveSessionsAsync(userInfoAccessor.UserId);
        return Results.Ok(sessions);
    }

    public static async Task<IResult> RevokeSession(
        long refreshTokenId,
        IUserSessionRepository sessionRepository,
        IUserInfoAccessor userInfoAccessor)
    {
        if (!userInfoAccessor.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        await sessionRepository.RevokeSessionAsync(refreshTokenId, userInfoAccessor.Email);
        return Results.Ok(new { success = true, message = "Session revoked successfully." });
    }
}
