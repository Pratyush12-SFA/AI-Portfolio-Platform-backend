using System.Security.Cryptography;
using AIPortfolio.Application.Abstractions;

namespace AIPortfolio.Infrastructure.Authentication;

internal sealed class RefreshTokenGenerator: IRefreshTokenGenerator
{
    public string Generate()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        
        return Convert.ToBase64String(bytes);
    }
}