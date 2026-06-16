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
}