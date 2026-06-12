using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.IsActive)
            .HasDefaultValue(true);

        builder.Property(user => user.CreatedAtUtc)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(user => user.Username)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@mail.com",
                PasswordHash = "PBKDF2$SHA1$100000$vV1m6V5tzeMRliH/rB450w==$JlDCqiKUqMI+aNNl1IU3ehFsAS5DH3JDEy61lcPKKkw=",
                FullName = "System Administrator",
                IsActive = true,
                CreatedAtUtc = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                Username = "operator",
                Email = "operator@mail.com",
                PasswordHash = "PBKDF2$SHA1$100000$7iHVnSCEfS0wYo3XuDAAgQ==$D+Jvtm+j7k2tygJNaXhYUatMAq5M1aOr29vydHMzlNk=",
                FullName = "Warehouse Operator",
                IsActive = true,
                CreatedAtUtc = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 3,
                Username = "auditor",
                Email = "auditor@mail.com",
                PasswordHash = "PBKDF2$SHA1$100000$ol6gFD9h7qxAfvgFtGBbTA==$4cR8MwnUTfTrQcJTFIFJHBZE8S91xPtaob3eZ3A6OU4=",
                FullName = "Inventory Auditor",
                IsActive = true,
                CreatedAtUtc = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
