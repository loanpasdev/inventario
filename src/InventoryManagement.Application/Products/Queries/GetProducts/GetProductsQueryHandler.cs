using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Products.Dtos;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductQueries _productQueries;

    public GetProductsQueryHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }

    public Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return _productQueries.GetProductsAsync(cancellationToken);
    }
}
