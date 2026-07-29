namespace AIPortfolio.Application.DTOs.Auth;

public sealed class RefreshTokenResponse
{
    public string AccessToken { get; init; } = null!;
    public required string RefreshToken { get; init; }
}