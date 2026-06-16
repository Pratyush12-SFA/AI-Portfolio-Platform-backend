namespace AIPortfolio.Application.DTOs.Auth;

public sealed class LogoutRequest
{
    public required string RefreshToken { get; init; }
}