using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Products.Dtos;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetInventoryValueByCategory;

public sealed class GetInventoryValueByCategoryQueryHandler : IRequestHandler<GetInventoryValueByCategoryQuery, IReadOnlyList<InventoryValueByCategoryDto>>
{
    private readonly IProductQueries _productQueries;

    public GetInventoryValueByCategoryQueryHandler(IProductQueries productQueries)
    {
        _productQueries = productQueries;
    }

    public Task<IReadOnlyList<InventoryValueByCategoryDto>> Handle(GetInventoryValueByCategoryQuery request, CancellationToken cancellationToken)
    {
        return _productQueries.GetInventoryValueByCategoryAsync(cancellationToken);
    }
}
