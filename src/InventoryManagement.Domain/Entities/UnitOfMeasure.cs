namespace InventoryManagement.Domain.Entities;

public class UnitOfMeasure
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Activo";
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
