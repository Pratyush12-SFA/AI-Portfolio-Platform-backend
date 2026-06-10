namespace AIPortfolio.Application.DTOs.Auth;

public sealed class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
}