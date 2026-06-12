using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");

        builder.HasKey(currency => currency.Id);

        builder.Property(currency => currency.Code)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(currency => currency.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(currency => currency.Status)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("Activo");

        builder.HasIndex(currency => currency.Code)
            .IsUnique();

        builder.HasData(
            new Currency { Id = 1, Code = "USD", Name = "Dolar estadounidense", Status = "Activo" },
            new Currency { Id = 2, Code = "VES", Name = "Bolivar", Status = "Activo" });
    }
}
