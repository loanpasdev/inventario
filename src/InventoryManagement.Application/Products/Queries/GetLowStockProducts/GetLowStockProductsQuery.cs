using InventoryManagement.Application.Products.Dtos;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetLowStockProducts;

public sealed record GetLowStockProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
