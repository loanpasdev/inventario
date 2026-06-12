namespace InventoryManagement.Application.UnitsOfMeasure.Dtos;

public sealed class UnitOfMeasureDto
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
