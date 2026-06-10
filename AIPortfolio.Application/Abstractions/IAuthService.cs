using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Abstractions;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}