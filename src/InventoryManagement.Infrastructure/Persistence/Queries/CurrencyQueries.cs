using Dapper;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Currencies.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Persistence.Queries;

public sealed class CurrencyQueries : ICurrencyQueries
{
    private readonly string _connectionString;

    public CurrencyQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection'.");
    }

    public async Task<IReadOnlyList<CurrencyDto>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                c.Id,
                c.Code,
                c.Name,
                c.Status
            FROM dbo.Currencies c
            ORDER BY c.Code;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var currencies = await connection.QueryAsync<CurrencyDto>(command);
        return currencies.ToList();
    }
}
