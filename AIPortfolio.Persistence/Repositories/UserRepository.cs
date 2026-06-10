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
}