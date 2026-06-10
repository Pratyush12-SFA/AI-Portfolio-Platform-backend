namespace AIPortfolio.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        long id,
        string email,
        string fullName);
}