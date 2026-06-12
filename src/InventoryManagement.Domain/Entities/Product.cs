namespace InventoryManagement.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = "00000000000000000000";
    public string? Description { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int MinimumStock { get; set; }
    public string Status { get; set; } = "Activo";
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
