using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Constants;
using Microsoft.AspNetCore.Http;

namespace AIPortfolio.Infrastructure.Authentication;

internal sealed class UserInfoAccessor : IUserInfoAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoAccessor(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId =>
        long.Parse(
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(UserClaimTypes.Id)?
                .Value ?? "0");

    public string Email =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirst(UserClaimTypes.Email)?
            .Value ?? string.Empty;

    public string FullName =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirst(UserClaimTypes.FullName)?
            .Value ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string GetUserName()
    {
        return FullName;
    }

    public string GetRemoteIp()
    {
        return _httpContextAccessor.HttpContext?
                   .Connection
                   .RemoteIpAddress?
                   .ToString()
               ?? "Unknown";
    }

    public string? GetUserAgent()
    {
        return _httpContextAccessor
            .HttpContext?
            .Request
            .Headers["User-Agent"]
            .ToString();
    }
}