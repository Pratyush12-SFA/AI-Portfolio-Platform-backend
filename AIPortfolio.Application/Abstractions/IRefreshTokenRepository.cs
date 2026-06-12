using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IRefreshTokenRepository
{
    Task<long> CreateAsync(
        RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(
        string token);

    Task RemoveAsync(
        string token,
        string revokedBy);
    
    Task RevokeAllAsync(
        long userId,
        string revokedBy);
}