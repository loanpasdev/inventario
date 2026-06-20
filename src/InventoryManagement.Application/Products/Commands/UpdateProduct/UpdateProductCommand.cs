using MediatR;

namespace InventoryManagement.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    int Id,
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
    int UnitOfMeasureId) : IRequest;
