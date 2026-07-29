using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly AIPortfolioDbContext _dbContext;

    public UserRepository(AIPortfolioDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<long> CreateAsync(User user, string createdBy, string createdFromIp)
    {
        user.CreatedBy = createdBy;
        user.CreatedFromIp = createdFromIp;
        user.CreatedOn = DateTime.UtcNow;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
    }

    public async Task<long> CreateGoogleUserAsync(User user, string createdBy, string createdFromIp)
    {
        user.CreatedBy = createdBy;
        user.CreatedFromIp = createdFromIp;
        user.CreatedOn = DateTime.UtcNow;
        user.IsEmailVerified = true;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task LinkGoogleAccountAsync(long userId, string googleId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.GoogleId = googleId;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateSecurityTokensAsync(
        long userId,
        string? resetToken,
        DateTime? resetExpires,
        string? verificationToken,
        DateTime? verificationExpires)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordExpiresAt = resetExpires;
            user.VerificationToken = verificationToken;
            user.VerificationExpiresAt = verificationExpires;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task VerifyEmailAsync(long userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.IsEmailVerified = true;
            user.VerificationToken = null;
            user.VerificationExpiresAt = null;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdatePasswordAsync(long userId, string passwordHash)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.PasswordHash = passwordHash;
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiresAt = null;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<User?> GetByResetTokenAsync(string token)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u =>
            u.ResetPasswordToken == token && u.ResetPasswordExpiresAt > DateTime.UtcNow);
    }

    public async Task<User?> GetByVerificationTokenAsync(string token)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u =>
            u.VerificationToken == token && u.VerificationExpiresAt > DateTime.UtcNow);
    }
}