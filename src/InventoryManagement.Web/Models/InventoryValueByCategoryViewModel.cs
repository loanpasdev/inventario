namespace InventoryManagement.Web.Models;

public sealed class InventoryValueByCategoryViewModel
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal TotalInventoryValue { get; init; }
}
