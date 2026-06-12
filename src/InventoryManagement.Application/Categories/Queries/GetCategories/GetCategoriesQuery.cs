using InventoryManagement.Application.Categories.Dtos;
using MediatR;

namespace InventoryManagement.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
