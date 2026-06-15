namespace AIPortfolio.Application.DTOs.Auth;

public sealed class RefreshTokenResponse
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}