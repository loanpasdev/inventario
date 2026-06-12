using Dapper;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Products.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Persistence.Queries;

public sealed class ProductQueries : IProductQueries
{
    private readonly string _connectionString;

    public ProductQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection'.");
    }

    public async Task<IReadOnlyList<ProductDto>> GetLowStockProductsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                p.Id,
                p.Name,
                p.Barcode,
                p.Description,
                c.Name AS Category,
                u.Code AS UnitOfMeasureCode,
                u.Name AS UnitOfMeasureName,
                p.CurrencyCode,
                p.PurchasePrice,
                p.SalePrice,
                CASE
                    WHEN p.CurrencyCode = 'USD' THEN p.PurchasePrice
                    ELSE ROUND(p.PurchasePrice / COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS PurchasePriceUsd,
                CASE
                    WHEN p.CurrencyCode = 'VES' THEN p.PurchasePrice
                    ELSE ROUND(p.PurchasePrice * COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS PurchasePriceVes,
                CASE
                    WHEN p.CurrencyCode = 'USD' THEN p.SalePrice
                    ELSE ROUND(p.SalePrice / COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS SalePriceUsd,
                CASE
                    WHEN p.CurrencyCode = 'VES' THEN p.SalePrice
                    ELSE ROUND(p.SalePrice * COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS SalePriceVes,
                COALESCE(rate.UsdToVesRate, 1) AS UsdToVesRate,
                p.Stock,
                p.MinimumStock,
                p.Status
            FROM dbo.Products p
            INNER JOIN dbo.Categories c ON c.Id = p.CategoryId
            INNER JOIN dbo.UnitsOfMeasure u ON u.Id = p.UnitOfMeasureId
            OUTER APPLY (
                SELECT TOP (1)
                    er.UsdToVesRate
                FROM dbo.ExchangeRates er
                WHERE er.IsActive = 1
                ORDER BY er.UpdatedAtUtc DESC, er.Id DESC
            ) rate
            WHERE p.Stock < p.MinimumStock
            ORDER BY p.Name;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var products = await connection.QueryAsync<ProductDto>(command);
        return products.ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                p.Id,
                p.Name,
                p.Barcode,
                p.Description,
                c.Name AS Category,
                u.Code AS UnitOfMeasureCode,
                u.Name AS UnitOfMeasureName,
                p.CurrencyCode,
                p.PurchasePrice,
                p.SalePrice,
                CASE
                    WHEN p.CurrencyCode = 'USD' THEN p.PurchasePrice
                    ELSE ROUND(p.PurchasePrice / COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS PurchasePriceUsd,
                CASE
                    WHEN p.CurrencyCode = 'VES' THEN p.PurchasePrice
                    ELSE ROUND(p.PurchasePrice * COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS PurchasePriceVes,
                CASE
                    WHEN p.CurrencyCode = 'USD' THEN p.SalePrice
                    ELSE ROUND(p.SalePrice / COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS SalePriceUsd,
                CASE
                    WHEN p.CurrencyCode = 'VES' THEN p.SalePrice
                    ELSE ROUND(p.SalePrice * COALESCE(NULLIF(rate.UsdToVesRate, 0), 1), 2)
                END AS SalePriceVes,
                COALESCE(rate.UsdToVesRate, 1) AS UsdToVesRate,
                p.Stock,
                p.MinimumStock,
                p.Status
            FROM dbo.Products p
            INNER JOIN dbo.Categories c ON c.Id = p.CategoryId
            INNER JOIN dbo.UnitsOfMeasure u ON u.Id = p.UnitOfMeasureId
            OUTER APPLY (
                SELECT TOP (1)
                    er.UsdToVesRate
                FROM dbo.ExchangeRates er
                WHERE er.IsActive = 1
                ORDER BY er.UpdatedAtUtc DESC, er.Id DESC
            ) rate
            ORDER BY p.Name;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var products = await connection.QueryAsync<ProductDto>(command);
        return products.ToList();
    }

    public async Task<IReadOnlyList<InventoryValueByCategoryDto>> GetInventoryValueByCategoryAsync(CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        var command = new CommandDefinition(
            "dbo.sp_GetInventoryValueByCategory",
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var rows = await connection.QueryAsync<InventoryValueByCategoryDto>(command);
        return rows.ToList();
    }
}
