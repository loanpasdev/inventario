using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Common.Interfaces;

public interface IMasterStatusRepository
{
    Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
    Task<UnitOfMeasure?> GetUnitOfMeasureByIdAsync(int id, CancellationToken cancellationToken);
    Task<Currency?> GetCurrencyByIdAsync(int id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
