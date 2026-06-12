using InventoryManagement.Application.Categories.Dtos;

namespace InventoryManagement.Application.Common.Interfaces;

public interface ICategoryQueries
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);
}

