namespace InventoryManagement.Web.Models;

public sealed class CurrencyListItemViewModel
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string DisplayName => $"{Name} ({Code})";
}
