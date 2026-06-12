using InventoryManagement.Application.Products.Dtos;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetInventoryValueByCategory;

public sealed record GetInventoryValueByCategoryQuery : IRequest<IReadOnlyList<InventoryValueByCategoryDto>>;
