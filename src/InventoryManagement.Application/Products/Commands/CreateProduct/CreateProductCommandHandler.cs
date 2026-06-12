using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductCommandRepository _productCommandRepository;

    public CreateProductCommandHandler(IProductCommandRepository productCommandRepository)
    {
        _productCommandRepository = productCommandRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _productCommandRepository.CategoryExistsAsync(request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            throw new InvalidOperationException($"La categoria con Id {request.CategoryId} no existe.");
        }

        var unitOfMeasureExists = await _productCommandRepository.UnitOfMeasureExistsAsync(request.UnitOfMeasureId, cancellationToken);
        if (!unitOfMeasureExists)
        {
            throw new InvalidOperationException($"La unidad de medida con Id {request.UnitOfMeasureId} no existe.");
        }

        var barcode = request.Barcode.Trim();
        if (barcode.Length != 20 || barcode.Any(character => !char.IsDigit(character)))
        {
            throw new InvalidOperationException("El código de barra debe tener exactamente 20 dígitos.");
        }

        var currencyCode = request.CurrencyCode.Trim().ToUpperInvariant();
        if (currencyCode is not ("USD" or "VES"))
        {
            throw new InvalidOperationException("La moneda debe ser USD o VES.");
        }

        if (request.PurchasePrice <= 0)
        {
            throw new InvalidOperationException("El precio de compra debe ser mayor a 0.");
        }

        if (request.SalePrice <= 0)
        {
            throw new InvalidOperationException("El precio de venta debe ser mayor a 0.");
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Barcode = barcode,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            CurrencyCode = currencyCode,
            PurchasePrice = request.PurchasePrice,
            SalePrice = request.SalePrice,
            Stock = request.Stock,
            MinimumStock = request.MinimumStock,
            Status = "Activo",
            CategoryId = request.CategoryId,
            UnitOfMeasureId = request.UnitOfMeasureId,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _productCommandRepository.AddAsync(product, cancellationToken);
        await _productCommandRepository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
