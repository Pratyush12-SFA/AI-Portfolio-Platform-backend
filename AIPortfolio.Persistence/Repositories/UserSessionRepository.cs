using System.Data;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;

namespace AIPortfolio.Persistence.Repositories;

internal sealed class UserSessionRepository : IUserSessionRepository
{
    private readonly DapperContext _dapperContext;

    public UserSessionRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<long> CreateSessionAsync(
        UserSession userSession)
    {
        using IDbConnection connection = _dapperContext.CreateConnection();
        
        return await connection.QuerySingleAsync<long>(
            "[Identity].[usp_UserSession_Create]",
            new
            {
                userSession.UserId,
                userSession.RefreshTokenId,
                userSession.CreatedFromIp,
                userSession.UserAgent,
                CreatedBy = userSession.CreatedBy ?? "System"
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task RevokeSessionAsync(
        long refreshTokenId,
        string revokedBy)
    {
        using IDbConnection connection =
            _dapperContext.CreateConnection();

        await connection.ExecuteAsync(
            "Identity.usp_UserSession_Revoke",
            new
            {
                RefreshTokenId = refreshTokenId,
                RevokedBy = revokedBy
            },
            commandType:
            CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<UserSession>>
        GetActiveSessionsAsync(
            long userId)
    {
        using IDbConnection connection =
            _dapperContext.CreateConnection();

        return await connection.QueryAsync<UserSession>(
            "Identity.usp_UserSession_GetActive",
            new
            {
                UserId = userId
            },
            commandType:
            CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<UserSession>>
        GetUserSessionsAsync(
            long userId)
    {
        using IDbConnection connection =
            _dapperContext.CreateConnection();

        return await connection.QueryAsync<UserSession>(
            "[Identity].[usp_UserSession_GetByUserId]",
            new
            {
                UserId = userId
            },
            commandType:
            CommandType.StoredProcedure);
    }
}