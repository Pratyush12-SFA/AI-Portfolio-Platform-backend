using AIPortfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AIPortfolio.Infrastructure.Authentication;

public sealed class CookieService : ICookieService
{
    private const string RefreshTokenCookieName = "refresh_token";

    public void SetRefreshTokenCookie(HttpResponse httpResponse, string refreshToken, bool rememberMe)
    {
        CookieOptions options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
        };
        if (rememberMe)
        {
            options.Expires = DateTimeOffset.Now.AddMinutes(30);
        }
        httpResponse.Cookies.Append(RefreshTokenCookieName, refreshToken, options);
        
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