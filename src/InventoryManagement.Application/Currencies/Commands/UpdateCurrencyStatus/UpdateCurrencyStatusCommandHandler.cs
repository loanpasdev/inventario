using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Currencies.Commands.UpdateCurrencyStatus;

public sealed class UpdateCurrencyStatusCommandHandler : IRequestHandler<UpdateCurrencyStatusCommand>
{
    private readonly IMasterStatusRepository _masterStatusRepository;

    public UpdateCurrencyStatusCommandHandler(IMasterStatusRepository masterStatusRepository)
    {
        _masterStatusRepository = masterStatusRepository;
    }

    public async Task<Unit> Handle(UpdateCurrencyStatusCommand request, CancellationToken cancellationToken)
    {
        var normalizedStatus = NormalizeStatus(request.Status);
        var currency = await _masterStatusRepository.GetCurrencyByIdAsync(request.Id, cancellationToken);

        if (currency is null)
        {
            throw new KeyNotFoundException($"La moneda con Id {request.Id} no existe.");
        }

        currency.Status = normalizedStatus;
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
