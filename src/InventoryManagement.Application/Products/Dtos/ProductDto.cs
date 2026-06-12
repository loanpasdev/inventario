namespace InventoryManagement.Application.Products.Dtos;

public sealed class ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Barcode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Category { get; init; } = string.Empty;
    public string UnitOfMeasureCode { get; init; } = string.Empty;
    public string UnitOfMeasureName { get; init; } = string.Empty;
    public string CurrencyCode { get; init; } = "USD";
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
    public decimal PurchasePriceUsd { get; init; }
    public decimal PurchasePriceVes { get; init; }
    public decimal SalePriceUsd { get; init; }
    public decimal SalePriceVes { get; init; }
    public decimal UsdToVesRate { get; init; }
    public int Stock { get; init; }
    public int MinimumStock { get; init; }
    public string Status { get; init; } = "Activo";
}
