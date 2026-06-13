using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.UnitsOfMeasure.Commands.UpdateUnitOfMeasureStatus;

public sealed class UpdateUnitOfMeasureStatusCommandHandler : IRequestHandler<UpdateUnitOfMeasureStatusCommand>
{
    private readonly IMasterStatusRepository _masterStatusRepository;

    public UpdateUnitOfMeasureStatusCommandHandler(IMasterStatusRepository masterStatusRepository)
    {
        _masterStatusRepository = masterStatusRepository;
    }

    public async Task<Unit> Handle(UpdateUnitOfMeasureStatusCommand request, CancellationToken cancellationToken)
    {
        var normalizedStatus = NormalizeStatus(request.Status);
        var unitOfMeasure = await _masterStatusRepository.GetUnitOfMeasureByIdAsync(request.Id, cancellationToken);

        if (unitOfMeasure is null)
        {
            throw new KeyNotFoundException($"La unidad de medida con Id {request.Id} no existe.");
        }

        unitOfMeasure.Status = normalizedStatus;
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
