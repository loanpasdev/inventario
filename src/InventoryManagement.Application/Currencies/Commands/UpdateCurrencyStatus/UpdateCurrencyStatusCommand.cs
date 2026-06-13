using MediatR;

namespace InventoryManagement.Application.Currencies.Commands.UpdateCurrencyStatus;

public sealed record UpdateCurrencyStatusCommand(int Id, string Status) : IRequest;
