namespace InventoryManagement.Domain.Entities;

public class ExchangeRate
{
    public int Id { get; set; }
    public decimal UsdToVesRate { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
