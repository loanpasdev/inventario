using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable("UnitsOfMeasure");

        builder.HasKey(unitOfMeasure => unitOfMeasure.Id);

        builder.Property(unitOfMeasure => unitOfMeasure.Code)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(unitOfMeasure => unitOfMeasure.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(unitOfMeasure => unitOfMeasure.Status)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("Activo");

        builder.HasIndex(unitOfMeasure => unitOfMeasure.Code)
            .IsUnique();

        builder.HasData(
            new UnitOfMeasure { Id = 1, Code = "UND", Name = "Unidad", Status = "Activo" },
            new UnitOfMeasure { Id = 2, Code = "KGR", Name = "Kilogramo", Status = "Activo" },
            new UnitOfMeasure { Id = 3, Code = "GRM", Name = "Gramo", Status = "Activo" },
            new UnitOfMeasure { Id = 4, Code = "LTR", Name = "Litro", Status = "Activo" },
            new UnitOfMeasure { Id = 5, Code = "MLT", Name = "Mililitro", Status = "Activo" },
            new UnitOfMeasure { Id = 6, Code = "CAJ", Name = "Caja", Status = "Activo" },
            new UnitOfMeasure { Id = 7, Code = "PAQ", Name = "Paquete", Status = "Activo" },
            new UnitOfMeasure { Id = 8, Code = "DOC", Name = "Docena", Status = "Activo" },
            new UnitOfMeasure { Id = 9, Code = "PAR", Name = "Par", Status = "Activo" },
            new UnitOfMeasure { Id = 10, Code = "MTR", Name = "Metro", Status = "Activo" },
            new UnitOfMeasure { Id = 11, Code = "CMT", Name = "Centimetro", Status = "Activo" },
            new UnitOfMeasure { Id = 12, Code = "MMT", Name = "Milimetro", Status = "Activo" },
            new UnitOfMeasure { Id = 13, Code = "MT2", Name = "Metro cuadrado", Status = "Activo" },
            new UnitOfMeasure { Id = 14, Code = "MT3", Name = "Metro cubico", Status = "Activo" },
            new UnitOfMeasure { Id = 15, Code = "BOT", Name = "Botella", Status = "Activo" },
            new UnitOfMeasure { Id = 16, Code = "LAT", Name = "Lata", Status = "Activo" },
            new UnitOfMeasure { Id = 17, Code = "ROL", Name = "Rollo", Status = "Activo" },
            new UnitOfMeasure { Id = 18, Code = "SAC", Name = "Saco", Status = "Activo" },
            new UnitOfMeasure { Id = 19, Code = "GAL", Name = "Galon", Status = "Activo" },
            new UnitOfMeasure { Id = 20, Code = "TON", Name = "Tonelada", Status = "Activo" });
    }
}
