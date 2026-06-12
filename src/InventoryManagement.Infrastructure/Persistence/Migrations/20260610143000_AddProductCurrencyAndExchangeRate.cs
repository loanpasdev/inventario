using System;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260610143000_AddProductCurrencyAndExchangeRate")]
    public partial class AddProductCurrencyAndExchangeRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Products",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "USD");

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsdToVesRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.Sql(
                """
                SET IDENTITY_INSERT dbo.ExchangeRates ON;

                INSERT INTO dbo.ExchangeRates (Id, UsdToVesRate, IsActive, UpdatedAtUtc)
                VALUES (1, 36.5000, 1, '2026-06-10T00:00:00.0000000Z');

                SET IDENTITY_INSERT dbo.ExchangeRates OFF;
                """);

            migrationBuilder.Sql(
                """
                UPDATE dbo.Products
                SET CurrencyCode = 'USD'
                WHERE CurrencyCode IS NULL OR LTRIM(RTRIM(CurrencyCode)) = '';
                """);

            migrationBuilder.Sql(
                """
                CREATE OR ALTER PROCEDURE dbo.sp_GetInventoryValueByCategory
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @UsdToVesRate decimal(18,4) =
                    (
                        SELECT TOP (1) er.UsdToVesRate
                        FROM dbo.ExchangeRates er
                        WHERE er.IsActive = 1
                        ORDER BY er.UpdatedAtUtc DESC, er.Id DESC
                    );

                    SET @UsdToVesRate = COALESCE(NULLIF(@UsdToVesRate, 0), 1);

                    SELECT
                        c.Id AS CategoryId,
                        c.Name AS CategoryName,
                        SUM(
                            (
                                CASE
                                    WHEN p.CurrencyCode = 'USD' THEN p.SalePrice * @UsdToVesRate
                                    ELSE p.SalePrice
                                END
                            ) * p.Stock
                        ) AS TotalInventoryValue
                    FROM dbo.Categories c
                    INNER JOIN dbo.Products p ON p.CategoryId = c.Id
                    GROUP BY c.Id, c.Name
                    ORDER BY c.Name;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR ALTER PROCEDURE dbo.sp_GetInventoryValueByCategory
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        c.Id AS CategoryId,
                        c.Name AS CategoryName,
                        SUM(p.SalePrice * p.Stock) AS TotalInventoryValue
                    FROM dbo.Categories c
                    INNER JOIN dbo.Products p ON p.CategoryId = c.Id
                    GROUP BY c.Id, c.Name
                    ORDER BY c.Name;
                END
                """);

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Products");
        }
    }
}
