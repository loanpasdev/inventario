using InventoryManagement.Application.UnitsOfMeasure.Dtos;
using MediatR;

namespace InventoryManagement.Application.UnitsOfMeasure.Queries.GetUnitsOfMeasure;

public sealed record GetUnitsOfMeasureQuery : IRequest<IReadOnlyList<UnitOfMeasureDto>>;
