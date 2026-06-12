using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Seeding;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        var desiredUnits = new Dictionary<int, (string Code, string Name)>
        {
            [1] = ("UND", "Unidad"),
            [2] = ("KGR", "Kilogramo"),
            [3] = ("GRM", "Gramo"),
            [4] = ("LTR", "Litro"),
            [5] = ("MLT", "Mililitro"),
            [6] = ("CAJ", "Caja"),
            [7] = ("PAQ", "Paquete"),
            [8] = ("DOC", "Docena"),
            [9] = ("PAR", "Par"),
            [10] = ("MTR", "Metro"),
            [11] = ("CMT", "Centimetro"),
            [12] = ("MMT", "Milimetro"),
            [13] = ("MT2", "Metro cuadrado"),
            [14] = ("MT3", "Metro cubico"),
            [15] = ("BOT", "Botella"),
            [16] = ("LAT", "Lata"),
            [17] = ("ROL", "Rollo"),
            [18] = ("SAC", "Saco"),
            [19] = ("GAL", "Galon"),
            [20] = ("TON", "Tonelada")
        };

        var existingUnits = await context.UnitsOfMeasure.ToListAsync(cancellationToken);
        foreach (var unit in existingUnits)
        {
            if (!desiredUnits.TryGetValue(unit.Id, out var desiredUnit))
            {
                continue;
            }

            if (!string.Equals(unit.Code, desiredUnit.Code, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(unit.Name, desiredUnit.Name, StringComparison.Ordinal))
            {
                unit.Code = desiredUnit.Code;
                unit.Name = desiredUnit.Name;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        var desiredUsdToVesRate = 572.6800m;
        var activeRate = await context.ExchangeRates
            .OrderByDescending(rate => rate.UpdatedAtUtc)
            .ThenByDescending(rate => rate.Id)
            .FirstOrDefaultAsync(rate => rate.IsActive, cancellationToken);

        if (activeRate is null)
        {
            context.ExchangeRates.Add(new ExchangeRate
            {
                UsdToVesRate = desiredUsdToVesRate,
                IsActive = true,
                UpdatedAtUtc = DateTime.UtcNow
            });
            await context.SaveChangesAsync(cancellationToken);
        }
        else if (activeRate.UsdToVesRate != desiredUsdToVesRate)
        {
            activeRate.UsdToVesRate = desiredUsdToVesRate;
            activeRate.UpdatedAtUtc = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }

        var existingProductsCount = await context.Products.CountAsync(cancellationToken);
        var desiredCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Perifericos",
            "Monitores",
            "Accesorios",
            "Componentes",
            "Almacenamiento",
            "Redes"
        };

        var reseedNeeded = existingProductsCount < 15;
        if (!reseedNeeded && existingProductsCount > 0)
        {
            var existingProductCategoryNames = await context.Products
                .Select(product => product.Category!.Name)
                .Distinct()
                .ToListAsync(cancellationToken);

            reseedNeeded = existingProductCategoryNames.Any(categoryName => !desiredCategories.Contains(categoryName));
        }

        if (!reseedNeeded)
        {
            return;
        }

        if (existingProductsCount > 0)
        {
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync(cancellationToken);
        }

        var categoriesToEnsure = new[]
        {
            "Perifericos",
            "Monitores",
            "Accesorios",
            "Componentes",
            "Almacenamiento",
            "Redes"
        };

        var existingCategoryNames = await context.Categories
            .Select(category => category.Name)
            .ToListAsync(cancellationToken);

        foreach (var categoryName in categoriesToEnsure)
        {
            if (!existingCategoryNames.Contains(categoryName, StringComparer.OrdinalIgnoreCase))
            {
                context.Categories.Add(new Category { Name = categoryName });
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        var categories = await context.Categories.ToListAsync(cancellationToken);
        var categoriesByName = categories.ToDictionary(category => category.Name, StringComparer.OrdinalIgnoreCase);

        var units = await context.UnitsOfMeasure.ToListAsync(cancellationToken);
        var unitsByCode = units.ToDictionary(unit => unit.Code, StringComparer.OrdinalIgnoreCase);

        if (!categoriesByName.ContainsKey("Perifericos")
            || !categoriesByName.ContainsKey("Monitores")
            || !categoriesByName.ContainsKey("Accesorios")
            || !categoriesByName.ContainsKey("Componentes")
            || !categoriesByName.ContainsKey("Almacenamiento")
            || !categoriesByName.ContainsKey("Redes"))
        {
            throw new InvalidOperationException("No fue posible preparar las categorías para el sembrado.");
        }

        if (!unitsByCode.ContainsKey("UND")
            || !unitsByCode.ContainsKey("MTR")
            || !unitsByCode.ContainsKey("MLT")
            || !unitsByCode.ContainsKey("GRM")
            || !unitsByCode.ContainsKey("PAQ"))
        {
            throw new InvalidOperationException("No fue posible preparar las unidades de medida para el sembrado.");
        }

        var products = new List<Product>
        {
            new()
            {
                Name = "Teclado mecánico",
                Barcode = "10000000000000002001",
                Description = "Switches azules, retroiluminado.",
                CategoryId = categoriesByName["Perifericos"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 18.00m,
                SalePrice = 25.00m,
                Stock = 32,
                MinimumStock = 8
            },
            new()
            {
                Name = "Mouse inalámbrico",
                Barcode = "10000000000000002002",
                Description = "2.4G, sensor óptico.",
                CategoryId = categoriesByName["Perifericos"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 9.50m,
                SalePrice = 14.99m,
                Stock = 3,
                MinimumStock = 12
            },
            new()
            {
                Name = "Memoria RAM 8GB DDR4",
                Barcode = "10000000000000002003",
                Description = "3200MHz, 1 módulo.",
                CategoryId = categoriesByName["Componentes"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 16.00m,
                SalePrice = 22.00m,
                Stock = 20,
                MinimumStock = 6
            },
            new()
            {
                Name = "Fuente de poder 600W",
                Barcode = "10000000000000002004",
                Description = "80+ Bronze.",
                CategoryId = categoriesByName["Componentes"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 32.00m,
                SalePrice = 45.00m,
                Stock = 1,
                MinimumStock = 4
            },
            new()
            {
                Name = "Monitor 24\" IPS",
                Barcode = "10000000000000002005",
                Description = "1080p, 75Hz.",
                CategoryId = categoriesByName["Monitores"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 85.00m,
                SalePrice = 115.00m,
                Stock = 1,
                MinimumStock = 3
            },
            new()
            {
                Name = "Monitor 27\" 2K",
                Barcode = "10000000000000002006",
                Description = "QHD, 144Hz.",
                CategoryId = categoriesByName["Monitores"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 165.00m,
                SalePrice = 219.00m,
                Stock = 6,
                MinimumStock = 2
            },
            new()
            {
                Name = "SSD 500GB SATA",
                Barcode = "10000000000000002007",
                Description = "2.5\", 500GB.",
                CategoryId = categoriesByName["Almacenamiento"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 24.00m,
                SalePrice = 32.00m,
                Stock = 14,
                MinimumStock = 6
            },
            new()
            {
                Name = "HDD 1TB",
                Barcode = "10000000000000002008",
                Description = "3.5\", 7200RPM.",
                CategoryId = categoriesByName["Almacenamiento"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 27.00m,
                SalePrice = 39.00m,
                Stock = 2,
                MinimumStock = 6
            },
            new()
            {
                Name = "Cable HDMI 2m",
                Barcode = "10000000000000002009",
                Description = "Alta velocidad, 2 metros.",
                CategoryId = categoriesByName["Accesorios"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "VES",
                PurchasePrice = 170.00m,
                SalePrice = 240.00m,
                Stock = 40,
                MinimumStock = 10
            },
            new()
            {
                Name = "Cable de red Cat6 (10m)",
                Barcode = "10000000000000002010",
                Description = "UTP Cat6, 10 metros.",
                CategoryId = categoriesByName["Redes"].Id,
                UnitOfMeasureId = unitsByCode["MTR"].Id,
                CurrencyCode = "VES",
                PurchasePrice = 32.00m,
                SalePrice = 49.00m,
                Stock = 150,
                MinimumStock = 60
            },
            new()
            {
                Name = "Router WiFi AC1200",
                Barcode = "10000000000000002011",
                Description = "Doble banda 2.4/5GHz.",
                CategoryId = categoriesByName["Redes"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 19.00m,
                SalePrice = 29.00m,
                Stock = 9,
                MinimumStock = 3
            },
            new()
            {
                Name = "Adaptador USB WiFi",
                Barcode = "10000000000000002012",
                Description = "AC600, alta ganancia.",
                CategoryId = categoriesByName["Redes"].Id,
                UnitOfMeasureId = unitsByCode["UND"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 6.00m,
                SalePrice = 9.50m,
                Stock = 2,
                MinimumStock = 8
            },
            new()
            {
                Name = "Pasta térmica 1g",
                Barcode = "10000000000000002013",
                Description = "Para CPU/GPU.",
                CategoryId = categoriesByName["Accesorios"].Id,
                UnitOfMeasureId = unitsByCode["GRM"].Id,
                CurrencyCode = "VES",
                PurchasePrice = 60.00m,
                SalePrice = 90.00m,
                Stock = 35,
                MinimumStock = 12
            },
            new()
            {
                Name = "Aire comprimido 400ml",
                Barcode = "10000000000000002014",
                Description = "Limpieza de teclado/PC.",
                CategoryId = categoriesByName["Accesorios"].Id,
                UnitOfMeasureId = unitsByCode["MLT"].Id,
                CurrencyCode = "VES",
                PurchasePrice = 210.00m,
                SalePrice = 280.00m,
                Stock = 18,
                MinimumStock = 6
            },
            new()
            {
                Name = "Organizador de cables (pack)",
                Barcode = "10000000000000002015",
                Description = "Pack de 10 unidades.",
                CategoryId = categoriesByName["Accesorios"].Id,
                UnitOfMeasureId = unitsByCode["PAQ"].Id,
                CurrencyCode = "USD",
                PurchasePrice = 1.80m,
                SalePrice = 3.00m,
                Stock = 6,
                MinimumStock = 12
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync(cancellationToken);
    }
}

