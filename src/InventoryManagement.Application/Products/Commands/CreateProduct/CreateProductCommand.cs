using MediatR;

namespace InventoryManagement.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Barcode,
    string? Description,
    string CurrencyCode,
    decimal PurchasePrice,
    decimal SalePrice,
    int Stock,
    int MinimumStock,
    string Status,
    int CategoryId,
    int UnitOfMeasureId) : IRequest<int>;
