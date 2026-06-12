namespace InventoryManagement.Application.Products.Dtos;

public sealed class InventoryValueByCategoryDto
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal TotalInventoryValue { get; init; }
}
