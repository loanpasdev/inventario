using InventoryManagement.Application.Products.Dtos;

namespace InventoryManagement.Application.Common.Interfaces;

public interface IProductQueries
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductDto>> GetLowStockProductsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<InventoryValueByCategoryDto>> GetInventoryValueByCategoryAsync(CancellationToken cancellationToken);
}
