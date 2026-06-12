namespace InventoryManagement.Web.Models;

public sealed class CategoryListItemViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
