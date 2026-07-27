using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites;
using AIPortfolio.Persistence.Connections;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AIPortfolio.Persistence.Repositories;

public sealed class PromptRepository : IPromptRepository
{
    private readonly DapperContext _context;

    public PromptRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PromptTemplate?> GetActiveTemplateByFeatureAsync(string feature)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<PromptTemplate>(
            "Portfolio.usp_PromptTemplate_GetByFeature",
            new { Feature = feature },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> CreateTemplateAsync(PromptTemplate template)
    {
        using IDbConnection connection = _context.CreateConnection();
        string sql = @"
            INSERT INTO Portfolio.PromptTemplates (Feature, Version, SystemPrompt, UserPromptTemplate, IsActive, CreatedOn)
            VALUES (@Feature, @Version, @SystemPrompt, @UserPromptTemplate, @IsActive, SYSUTCDATETIME());
            SELECT SCOPE_IDENTITY();";
        
        return await connection.ExecuteScalarAsync<long>(sql, template);
    }
}
