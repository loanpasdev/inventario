using MediatR;

namespace InventoryManagement.Application.UnitsOfMeasure.Commands.UpdateUnitOfMeasureStatus;

public sealed record UpdateUnitOfMeasureStatusCommand(int Id, string Status) : IRequest;
