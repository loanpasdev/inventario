using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Infrastructure.Persistence.Migrations;

[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(ApplicationDbContext))]
[Migration("20260610180000_AddProductStatus")]
public partial class AddProductStatus : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "Products",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "Activo");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Status",
            table: "Products");
    }
}
