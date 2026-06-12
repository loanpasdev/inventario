using Dapper;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.UnitsOfMeasure.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Persistence.Queries;

public sealed class UnitOfMeasureQueries : IUnitOfMeasureQueries
{
    private readonly string _connectionString;

    public UnitOfMeasureQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection'.");
    }

    public async Task<IReadOnlyList<UnitOfMeasureDto>> GetUnitsOfMeasureAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                u.Id,
                u.Code,
                u.Name,
                u.Status
            FROM dbo.UnitsOfMeasure u
            ORDER BY u.Code;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var unitsOfMeasure = await connection.QueryAsync<UnitOfMeasureDto>(command);
        return unitsOfMeasure.ToList();
    }
}
