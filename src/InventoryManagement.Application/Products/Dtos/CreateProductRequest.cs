namespace InventoryManagement.Application.Products.Dtos;

public sealed class CreateProductRequest
{
    public string Name { get; init; } = string.Empty;
    public string Barcode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string CurrencyCode { get; init; } = "USD";
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
    public int Stock { get; init; }
    public int MinimumStock { get; init; }
    public string Status { get; init; } = "Activo";
    public int CategoryId { get; init; }
    public int UnitOfMeasureId { get; init; }
}
