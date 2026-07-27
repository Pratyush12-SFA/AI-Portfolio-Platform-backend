using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AIPortfolio.Persistence.Repositories;

public sealed class AIUsageRepository : IAIUsageRepository
{
    private readonly DapperContext _context;

    public AIUsageRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<long> CreateAsync(AIUsage usage)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(
            "Portfolio.usp_AIUsage_Create",
            new
            {
                UserId = usage.UserId,
                Feature = usage.Feature,
                Model = usage.Model,
                PromptType = usage.PromptType,
                InputTokens = usage.InputTokens,
                OutputTokens = usage.OutputTokens,
                EstimatedCost = usage.EstimatedCost,
                DurationMs = usage.DurationMs,
                Status = usage.Status
            },
            commandType: CommandType.StoredProcedure);
    }
}
