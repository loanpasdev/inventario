using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260611094000_AddMasterStatusesAndCurrencies")]
    public partial class AddMasterStatusesAndCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Categories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Activo");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UnitsOfMeasure",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Activo");

            migrationBuilder.DropIndex(
                name: "IX_UnitsOfMeasure_Code",
                table: "UnitsOfMeasure");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UnitsOfMeasure",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.Sql("""
                UPDATE dbo.Categories
                SET Status = 'Activo'
                WHERE Status IS NULL OR LTRIM(RTRIM(Status)) = '';

                UPDATE dbo.UnitsOfMeasure
                SET Status = 'Activo'
                WHERE Status IS NULL OR LTRIM(RTRIM(Status)) = '';

                UPDATE dbo.UnitsOfMeasure SET Code = 'UND' WHERE Id = 1;
                UPDATE dbo.UnitsOfMeasure SET Code = 'KGR' WHERE Id = 2;
                UPDATE dbo.UnitsOfMeasure SET Code = 'GRM' WHERE Id = 3;
                UPDATE dbo.UnitsOfMeasure SET Code = 'LTR' WHERE Id = 4;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MLT' WHERE Id = 5;
                UPDATE dbo.UnitsOfMeasure SET Code = 'CAJ' WHERE Id = 6;
                UPDATE dbo.UnitsOfMeasure SET Code = 'PAQ' WHERE Id = 7;
                UPDATE dbo.UnitsOfMeasure SET Code = 'DOC' WHERE Id = 8;
                UPDATE dbo.UnitsOfMeasure SET Code = 'PAR' WHERE Id = 9;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MTR' WHERE Id = 10;
                UPDATE dbo.UnitsOfMeasure SET Code = 'CMT' WHERE Id = 11;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MMT' WHERE Id = 12;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MT2' WHERE Id = 13;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MT3' WHERE Id = 14;
                UPDATE dbo.UnitsOfMeasure SET Code = 'BOT' WHERE Id = 15;
                UPDATE dbo.UnitsOfMeasure SET Code = 'LAT' WHERE Id = 16;
                UPDATE dbo.UnitsOfMeasure SET Code = 'ROL' WHERE Id = 17;
                UPDATE dbo.UnitsOfMeasure SET Code = 'SAC' WHERE Id = 18;
                UPDATE dbo.UnitsOfMeasure SET Code = 'GAL' WHERE Id = 19;
                UPDATE dbo.UnitsOfMeasure SET Code = 'TON' WHERE Id = 20;
                """);

            migrationBuilder.Sql("""
                SET IDENTITY_INSERT dbo.Currencies ON;

                INSERT INTO dbo.Currencies (Id, Code, Name, Status)
                VALUES
                    (1, 'USD', 'Dolar estadounidense', 'Activo'),
                    (2, 'VES', 'Bolivar', 'Activo');

                SET IDENTITY_INSERT dbo.Currencies OFF;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_Code",
                table: "UnitsOfMeasure",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_UnitsOfMeasure_Code",
                table: "UnitsOfMeasure");

            migrationBuilder.Sql("""
                UPDATE dbo.UnitsOfMeasure SET Code = 'UN' WHERE Id = 1;
                UPDATE dbo.UnitsOfMeasure SET Code = 'KG' WHERE Id = 2;
                UPDATE dbo.UnitsOfMeasure SET Code = 'G' WHERE Id = 3;
                UPDATE dbo.UnitsOfMeasure SET Code = 'LT' WHERE Id = 4;
                UPDATE dbo.UnitsOfMeasure SET Code = 'ML' WHERE Id = 5;
                UPDATE dbo.UnitsOfMeasure SET Code = 'CAJ' WHERE Id = 6;
                UPDATE dbo.UnitsOfMeasure SET Code = 'PAQ' WHERE Id = 7;
                UPDATE dbo.UnitsOfMeasure SET Code = 'DOC' WHERE Id = 8;
                UPDATE dbo.UnitsOfMeasure SET Code = 'PAR' WHERE Id = 9;
                UPDATE dbo.UnitsOfMeasure SET Code = 'M' WHERE Id = 10;
                UPDATE dbo.UnitsOfMeasure SET Code = 'CM' WHERE Id = 11;
                UPDATE dbo.UnitsOfMeasure SET Code = 'MM' WHERE Id = 12;
                UPDATE dbo.UnitsOfMeasure SET Code = 'M2' WHERE Id = 13;
                UPDATE dbo.UnitsOfMeasure SET Code = 'M3' WHERE Id = 14;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UnitsOfMeasure",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_Code",
                table: "UnitsOfMeasure",
                column: "Code",
                unique: true);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UnitsOfMeasure");
        }
    }
}
