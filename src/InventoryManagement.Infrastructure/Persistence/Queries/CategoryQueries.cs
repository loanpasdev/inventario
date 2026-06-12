using Dapper;
using InventoryManagement.Application.Categories.Dtos;
using InventoryManagement.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Persistence.Queries;

public sealed class CategoryQueries : ICategoryQueries
{
    private readonly string _connectionString;

    public CategoryQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection'.");
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                c.Id,
                c.Name,
                c.Status
            FROM dbo.Categories c
            ORDER BY c.Name;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var categories = await connection.QueryAsync<CategoryDto>(command);
        return categories.ToList();
    }
}

