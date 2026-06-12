using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUnitOfMeasureAndPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "SalePrice");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasureId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnitsOfMeasure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeasure", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "UN", "Unidad" },
                    { 2, "KG", "Kilogramo" },
                    { 3, "G", "Gramo" },
                    { 4, "LT", "Litro" },
                    { 5, "ML", "Mililitro" },
                    { 6, "CAJ", "Caja" },
                    { 7, "PAQ", "Paquete" },
                    { 8, "DOC", "Docena" },
                    { 9, "PAR", "Par" },
                    { 10, "M", "Metro" },
                    { 11, "CM", "Centimetro" },
                    { 12, "MM", "Milimetro" },
                    { 13, "M2", "Metro cuadrado" },
                    { 14, "M3", "Metro cubico" },
                    { 15, "BOT", "Botella" },
                    { 16, "LAT", "Lata" },
                    { 17, "ROL", "Rollo" },
                    { 18, "SAC", "Saco" },
                    { 19, "GAL", "Galon" },
                    { 20, "TON", "Tonelada" }
                });

            migrationBuilder.Sql(
                """
                UPDATE dbo.Products SET PurchasePrice = 52.00, UnitOfMeasureId = 1 WHERE Id = 1;
                UPDATE dbo.Products SET PurchasePrice = 140.00, UnitOfMeasureId = 1 WHERE Id = 2;
                UPDATE dbo.Products SET PurchasePrice = 15.75, UnitOfMeasureId = 1 WHERE Id = 3;
                UPDATE dbo.Products
                SET PurchasePrice = CASE
                        WHEN SalePrice IS NULL OR SalePrice <= 0 THEN 0.01
                        ELSE SalePrice
                    END
                WHERE PurchasePrice IS NULL;

                UPDATE dbo.Products
                SET UnitOfMeasureId = 1
                WHERE UnitOfMeasureId IS NULL;
                """);

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitOfMeasureId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitOfMeasureId",
                table: "Products",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_Code",
                table: "UnitsOfMeasure",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitsOfMeasure_UnitOfMeasureId",
                table: "Products",
                column: "UnitOfMeasureId",
                principalTable: "UnitsOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitsOfMeasure_UnitOfMeasureId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "UnitsOfMeasure");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitOfMeasureId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasureId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.Sql(
                """
                CREATE OR ALTER PROCEDURE dbo.sp_GetInventoryValueByCategory
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        c.Id AS CategoryId,
                        c.Name AS CategoryName,
                        SUM(p.Price * p.Stock) AS TotalInventoryValue
                    FROM dbo.Categories c
                    INNER JOIN dbo.Products p ON p.CategoryId = c.Id
                    GROUP BY c.Id, c.Name
                    ORDER BY c.Name;
                END
                """);
        }
    }
}
