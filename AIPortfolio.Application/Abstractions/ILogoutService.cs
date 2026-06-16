using AIPortfolio.Application.DTOs.Auth;

namespace AIPortfolio.Application.Abstractions;

public interface ILogoutService
{
    Task LogoutAsync(LogoutRequest logoutRequest);
}