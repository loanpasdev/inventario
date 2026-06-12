using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(product => product.Barcode)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("00000000000000000000");

        builder.Property(product => product.Description)
            .HasMaxLength(500);

        builder.Property(product => product.CurrencyCode)
            .HasMaxLength(3)
            .IsRequired()
            .HasDefaultValue("USD");

        builder.Property(product => product.PurchasePrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(product => product.SalePrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(product => product.Status)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("Activo");

        builder.Property(product => product.CreatedAtUtc)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(product => product.UnitOfMeasure)
            .WithMany(unitOfMeasure => unitOfMeasure.Products)
            .HasForeignKey(product => product.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(product => product.CategoryId);
        builder.HasIndex(product => product.UnitOfMeasureId);
        builder.HasIndex(product => new { product.Stock, product.MinimumStock });

        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Teclado mecanico",
                Barcode = "10000000000000000001",
                Description = "Teclado con switch blue",
                CurrencyCode = "USD",
                PurchasePrice = 52.00m,
                SalePrice = 75.50m,
                Stock = 4,
                MinimumStock = 10,
                Status = "Activo",
                CategoryId = 1,
                UnitOfMeasureId = 1,
                CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Monitor 24 pulgadas",
                Barcode = "10000000000000000002",
                Description = "Panel IPS Full HD",
                CurrencyCode = "USD",
                PurchasePrice = 140.00m,
                SalePrice = 189.99m,
                Stock = 2,
                MinimumStock = 5,
                Status = "Activo",
                CategoryId = 2,
                UnitOfMeasureId = 1,
                CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "Mouse inalambrico",
                Barcode = "10000000000000000003",
                Description = "Mouse ergonomico",
                CurrencyCode = "USD",
                PurchasePrice = 15.75m,
                SalePrice = 24.90m,
                Stock = 15,
                MinimumStock = 8,
                Status = "Activo",
                CategoryId = 3,
                UnitOfMeasureId = 1,
                CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
