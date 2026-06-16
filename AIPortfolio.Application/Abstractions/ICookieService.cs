using Microsoft.AspNetCore.Http;

namespace AIPortfolio.Application.Abstractions;

public interface ICookieService
{
    void SetRefreshTokenCookie(
        HttpResponse httpResponse,
        string refreshToken);

    string? GetRefreshTokenCookie(
        HttpRequest httpRequest);

    void DeleteRefreshTokenCookie(
        HttpResponse httpResponse);
}