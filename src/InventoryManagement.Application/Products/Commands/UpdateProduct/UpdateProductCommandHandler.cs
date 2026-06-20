using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductCommandRepository _productCommandRepository;

    public UpdateProductCommandHandler(IProductCommandRepository productCommandRepository)
    {
        _productCommandRepository = productCommandRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productCommandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            throw new KeyNotFoundException($"El producto con Id {request.Id} no existe.");
        }

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
            throw new InvalidOperationException("El codigo de barra debe tener exactamente 20 digitos.");
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

        product.Name = request.Name.Trim();
        product.Barcode = barcode;
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        product.CurrencyCode = currencyCode;
        product.PurchasePrice = request.PurchasePrice;
        product.SalePrice = request.SalePrice;
        product.Stock = request.Stock;
        product.MinimumStock = request.MinimumStock;
        product.Status = request.Status;
        product.CategoryId = request.CategoryId;
        product.UnitOfMeasureId = request.UnitOfMeasureId;

        await _productCommandRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
