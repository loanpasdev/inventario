using InventoryManagement.Application.Categories.Dtos;
using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryQueries _categoryQueries;

    public GetCategoriesQueryHandler(ICategoryQueries categoryQueries)
    {
        _categoryQueries = categoryQueries;
    }

    public Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _categoryQueries.GetCategoriesAsync(cancellationToken);
    }
}

