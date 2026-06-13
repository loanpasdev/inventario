using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Categories.Commands.UpdateCategoryStatus;

public sealed class UpdateCategoryStatusCommandHandler : IRequestHandler<UpdateCategoryStatusCommand>
{
    private readonly IMasterStatusRepository _masterStatusRepository;

    public UpdateCategoryStatusCommandHandler(IMasterStatusRepository masterStatusRepository)
    {
        _masterStatusRepository = masterStatusRepository;
    }

    public async Task<Unit> Handle(UpdateCategoryStatusCommand request, CancellationToken cancellationToken)
    {
        var normalizedStatus = NormalizeStatus(request.Status);
        var category = await _masterStatusRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            throw new KeyNotFoundException($"La categoría con Id {request.Id} no existe.");
        }

        category.Status = normalizedStatus;
        await _masterStatusRepository.SaveChangesAsync(cancellationToken);

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
