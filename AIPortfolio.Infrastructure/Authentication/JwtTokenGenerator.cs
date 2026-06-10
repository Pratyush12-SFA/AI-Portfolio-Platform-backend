using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Constants;
using AIPortfolio.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AIPortfolio.Infrastructure.Authentication;

internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(
        IOptions<JwtSettings> settings)

    {
        _settings = settings.Value;
    }

    public string GenerateToken(
        long id,
        string email,
        string fullName)
    {
        Claim[] claims =
        {
            new(UserClaimTypes.Id, id.ToString()),
            new(UserClaimTypes.Email, email),
            new(UserClaimTypes.FullName, fullName),
            new(ClaimTypes.Name, fullName)
        };

        SigningCredentials credentials = new(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _settings.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}