using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Products.Commands.UpdateProductStatus;

public sealed class UpdateProductStatusCommandHandler : IRequestHandler<UpdateProductStatusCommand>
{
    private readonly IProductCommandRepository _productCommandRepository;

    public UpdateProductStatusCommandHandler(IProductCommandRepository productCommandRepository)
    {
        _productCommandRepository = productCommandRepository;
    }

    public async Task<Unit> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var normalizedStatus = NormalizeStatus(request.Status);
        var product = await _productCommandRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException($"El producto con Id {request.Id} no existe.");
        }

        product.Status = normalizedStatus;
        await _productCommandRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private static string NormalizeStatus(string status)
    {
        var normalizedStatus = status.Trim();
        if (normalizedStatus is not ("Activo" or "Inactivo"))
        {
            throw new InvalidOperationException("El estado debe ser Activo o Inactivo.");
        }

        return normalizedStatus;
    }
}
