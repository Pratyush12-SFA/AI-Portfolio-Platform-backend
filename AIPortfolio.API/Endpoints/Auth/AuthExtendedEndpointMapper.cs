using AIPortfolio.API.Common;
using AIPortfolio.API.Extensions;

namespace AIPortfolio.API.Endpoints.Auth;

internal sealed class AuthExtendedEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        ArgumentNullException.ThrowIfNull(endpointRouteBuilder);

        var authGroup = endpointRouteBuilder.MapAuthGroup();

        authGroup.MapPost("forgot-password", AuthExtendedEndpoint.ForgotPassword)
            .AllowAnonymous()
            .WithDisplayName("Forgot Password")
            .WithName("ForgotPassword")
            .WithDescription("Request password reset link");

        authGroup.MapPost("reset-password", AuthExtendedEndpoint.ResetPassword)
            .AllowAnonymous()
            .WithDisplayName("Reset Password")
            .WithName("ResetPassword")
            .WithDescription("Reset password using token");

        authGroup.MapPost("verify-email/request", AuthExtendedEndpoint.SendVerification)
            .RequireAuthorization()
            .WithDisplayName("Request Email Verification")
            .WithName("RequestEmailVerification")
            .WithDescription("Request email verification link");

        authGroup.MapPost("verify-email", AuthExtendedEndpoint.VerifyEmail)
            .AllowAnonymous()
            .WithDisplayName("Verify Email")
            .WithName("VerifyEmail")
            .WithDescription("Verify user email using token");

        authGroup.MapPost("change-password", AuthExtendedEndpoint.ChangePassword)
            .RequireAuthorization()
            .WithDisplayName("Change Password")
            .WithName("ChangePassword")
            .WithDescription("Change user password from dashboard");

        authGroup.MapGet("sessions", AuthExtendedEndpoint.GetActiveSessions)
            .RequireAuthorization()
            .WithDisplayName("Get Active Sessions")
            .WithName("GetActiveSessions")
            .WithDescription("Get list of active sessions for user");

        authGroup.MapDelete("sessions/{refreshTokenId:long}", AuthExtendedEndpoint.RevokeSession)
            .RequireAuthorization()
            .WithDisplayName("Revoke Session")
            .WithName("RevokeSession")
            .WithDescription("Revoke user session by refresh token ID");
    }
}