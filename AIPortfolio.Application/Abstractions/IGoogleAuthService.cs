using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Abstractions;

public interface IGoogleAuthService
{
    Task<LoginResponse?> LoginAsync(GoogleLoginRequest request);
}