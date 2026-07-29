namespace AIPortfolio.Application.DTOs.Auth;

public sealed class CurrentUserResponse
{
    public long UserId { get; init; }
    public string? Email { get; init; }
    public string? FullName { get; init; }
    public bool IsAuthenticated { get; init; }
}