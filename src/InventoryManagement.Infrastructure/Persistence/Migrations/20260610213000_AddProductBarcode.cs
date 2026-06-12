using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Infrastructure.Persistence.Migrations;

[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(ApplicationDbContext))]
[Migration("20260610213000_AddProductBarcode")]
public partial class AddProductBarcode : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Barcode",
            table: "Products",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "00000000000000000000");

        migrationBuilder.Sql("UPDATE dbo.Products SET Barcode = '10000000000000000001' WHERE Id = 1;");
        migrationBuilder.Sql("UPDATE dbo.Products SET Barcode = '10000000000000000002' WHERE Id = 2;");
        migrationBuilder.Sql("UPDATE dbo.Products SET Barcode = '10000000000000000003' WHERE Id = 3;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Barcode",
            table: "Products");
    }
}
