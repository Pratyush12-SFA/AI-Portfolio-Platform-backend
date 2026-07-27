using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;
public interface IUserInfoAccessor
{
    long UserId { get; }

    string Email { get; }

    string FullName { get; }

    bool IsAuthenticated { get; }
    string GetUserName();

    string GetRemoteIp();

    string? GetUserAgent();
}
