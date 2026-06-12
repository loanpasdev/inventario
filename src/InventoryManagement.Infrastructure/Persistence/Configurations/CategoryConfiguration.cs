using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(category => category.Status)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("Activo");

        builder.HasData(
            new Category { Id = 1, Name = "Perifericos", Status = "Activo" },
            new Category { Id = 2, Name = "Monitores", Status = "Activo" },
            new Category { Id = 3, Name = "Accesorios", Status = "Activo" });
    }
}
