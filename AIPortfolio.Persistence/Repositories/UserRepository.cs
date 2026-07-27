using System.Data;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(
        DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(
        string email)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<User>(
                "Identity.usp_User_Login",
                new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        using IDbConnection connection =
            _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "Identity.usp_User_GetById",
            new
            {
                Id = id
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using IDbConnection connection = _context.CreateConnection();

        return await connection.QuerySingleAsync<bool>(
            "Identity.usp_User_ExistsByEmail",
            new
            {
                Email = email
            },

            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> CreateAsync(User user,
        string createdBy, string createdFromIp)
    {
        using IDbConnection connection = _context.CreateConnection();
        
        return await connection.QuerySingleAsync<long>(
            "Identity.usp_User_Create",
            new
            {
                user.FullName,
                user.Email,
                user.PasswordHash,
                CreatedBy = createdBy,
                CreatedFromIp = createdFromIp
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        using IDbConnection connection =
            _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "Identity.usp_User_GetByGoogleId",
            new
            {
                GoogleId = googleId
            },
            commandType: CommandType.StoredProcedure);
        
    }

    public async Task<long> CreateGoogleUserAsync(User user,
        string createdBy, string createdFromIp)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<long>(
            "Identity.usp_User_CreateGoogleUser",
            new
            {
                user.FullName,
                user.Email,
                user.GoogleId,
                user.ProfilePictureUrl,
                CreatedBy = createdBy,
                CreatedFromIp = createdFromIp
            },
            commandType: CommandType.StoredProcedure);
    }
    public async Task LinkGoogleAccountAsync(
        long userId,
        string googleId)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            "Identity.usp_User_LinkGoogleAccount",
            new
            {
                UserId = userId,
                GoogleId = googleId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateSecurityTokensAsync(
        long userId,
        string? resetToken,
        DateTime? resetExpires,
        string? verificationToken,
        DateTime? verificationExpires)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE [Identity].[Users] SET ResetPasswordToken = @ResetPasswordToken, ResetPasswordExpiresAt = @ResetPasswordExpiresAt, VerificationToken = @VerificationToken, VerificationExpiresAt = @VerificationExpiresAt WHERE Id = @UserId",
            new
            {
                UserId = userId,
                ResetPasswordToken = resetToken,
                ResetPasswordExpiresAt = resetExpires,
                VerificationToken = verificationToken,
                VerificationExpiresAt = verificationExpires
            });
    }

    public async Task VerifyEmailAsync(long userId)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE [Identity].[Users] SET IsEmailVerified = 1, VerificationToken = NULL, VerificationExpiresAt = NULL WHERE Id = @UserId",
            new { UserId = userId });
    }

    public async Task UpdatePasswordAsync(long userId, string passwordHash)
    {
        using IDbConnection connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE [Identity].[Users] SET PasswordHash = @PasswordHash, ResetPasswordToken = NULL, ResetPasswordExpiresAt = NULL WHERE Id = @UserId",
            new { UserId = userId, PasswordHash = passwordHash });
    }

    public async Task<User?> GetByResetTokenAsync(string token)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM [Identity].[Users] WHERE ResetPasswordToken = @Token AND ResetPasswordExpiresAt > SYSUTCDATETIME()",
            new { Token = token });
    }

    public async Task<User?> GetByVerificationTokenAsync(string token)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM [Identity].[Users] WHERE VerificationToken = @Token AND VerificationExpiresAt > SYSUTCDATETIME()",
            new { Token = token });
    }
}