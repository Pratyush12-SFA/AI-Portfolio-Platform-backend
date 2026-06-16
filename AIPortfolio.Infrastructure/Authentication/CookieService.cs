using AIPortfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AIPortfolio.Infrastructure.Authentication;

public sealed class CookieService : ICookieService
{
    private const string RefreshTokenCookieName = "refresh_token";

    public void SetRefreshTokenCookie(HttpResponse httpResponse, string refreshToken)
    {
        httpResponse.Cookies.Append(RefreshTokenCookieName, refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
    }

    public string? GetRefreshTokenCookie(HttpRequest httpRequest)
    {
        httpRequest.Cookies.TryGetValue(RefreshTokenCookieName, out string? refreshToken);
        return refreshToken;
    }

    public void DeleteRefreshTokenCookie(HttpResponse httpResponse)
    {
        httpResponse.Cookies.Delete(RefreshTokenCookieName);
    }
}