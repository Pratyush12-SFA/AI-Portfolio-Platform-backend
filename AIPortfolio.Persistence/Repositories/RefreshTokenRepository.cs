using System.Data;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly DapperContext _context;

    public RefreshTokenRepository(
        DapperContext context)
    {
        _context = context;
    }

    public async Task<long> CreateAsync(
        RefreshToken refreshToken)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        return await connection
            .QuerySingleAsync<long>(
                "[Identity].[usp_RefreshToken_Create]",
                new
                {
                    refreshToken.UserId,
                    refreshToken.Token,
                    refreshToken.ExpiresAt,
                    refreshToken.CreatedBy,
                    refreshToken.CreatedFromIp
                },
                commandType:
                    CommandType.StoredProcedure);
    }

    public async Task<RefreshToken?>
        GetByTokenAsync(
            string token)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<
                RefreshToken>(
                "[Identity].[usp_RefreshToken_GetByToken]",
                new
                {
                    Token = token
                },
                commandType:
                    CommandType.StoredProcedure);
    }

    public async Task RemoveAsync(
        string token,
        string revokedBy)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            "[Identity].[usp_RefreshToken_Revoke]",
            new
            {
                Token = token,
                RevokedBy = revokedBy
            },
            commandType:
                CommandType.StoredProcedure);
    }

    public async Task RevokeAllAsync(
        long userId,
        string revokedBy)
    {
        using IDbConnection connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            "[Identity].[usp_RefreshToken_RevokeAll]",
            new
            {
                UserId = userId,
                RevokedBy = revokedBy
            },
            commandType:
                CommandType.StoredProcedure);
    }
}