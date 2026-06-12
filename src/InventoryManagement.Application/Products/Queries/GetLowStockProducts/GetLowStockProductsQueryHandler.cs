using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Products.Dtos;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetLowStockProducts;

public sealed class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductQueries _productQueries;

    public GetLowStockProductsQueryHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }

    public Task<IReadOnlyList<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        return _productQueries.GetLowStockProductsAsync(cancellationToken);
    }
}
