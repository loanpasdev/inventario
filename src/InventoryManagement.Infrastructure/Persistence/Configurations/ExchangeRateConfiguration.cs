using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("ExchangeRates");

        builder.HasKey(exchangeRate => exchangeRate.Id);

        builder.Property(exchangeRate => exchangeRate.UsdToVesRate)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(exchangeRate => exchangeRate.UpdatedAtUtc)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasData(
            new ExchangeRate
            {
                Id = 1,
                UsdToVesRate = 587.4059m,
                IsActive = true,
                UpdatedAtUtc = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
