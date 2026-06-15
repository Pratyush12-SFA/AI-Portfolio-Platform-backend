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
}