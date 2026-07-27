using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(long id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<long> CreateAsync(User user, string createdBy,
        string createdFromIp);
    Task<User?> GetByGoogleIdAsync(string googleId);

    Task<long> CreateGoogleUserAsync(
        User user,
        string createdBy,
        string createdFromIp);

    Task LinkGoogleAccountAsync(
        long id,
        string googleId);

    Task UpdateSecurityTokensAsync(
        long userId,
        string? resetToken,
        DateTime? resetExpires,
        string? verificationToken,
        DateTime? verificationExpires);

    Task VerifyEmailAsync(long userId);

    Task UpdatePasswordAsync(long userId, string passwordHash);

    Task<User?> GetByResetTokenAsync(string token);

    Task<User?> GetByVerificationTokenAsync(string token);
}