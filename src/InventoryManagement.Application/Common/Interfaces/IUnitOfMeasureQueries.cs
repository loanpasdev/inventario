using InventoryManagement.Application.UnitsOfMeasure.Dtos;

namespace InventoryManagement.Application.Common.Interfaces;

public interface IUnitOfMeasureQueries
{
    Task<IReadOnlyList<UnitOfMeasureDto>> GetUnitsOfMeasureAsync(CancellationToken cancellationToken);
}
