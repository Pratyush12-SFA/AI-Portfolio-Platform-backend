using System.Data;
using Microsoft.Data.SqlClient;

namespace AIPortfolio.Persistence.Connections;

public sealed class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}