namespace AIPortfolio.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public string Password { get; init; } = null!;  
}