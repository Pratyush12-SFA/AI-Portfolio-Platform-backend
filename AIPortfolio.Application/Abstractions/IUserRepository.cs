using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}