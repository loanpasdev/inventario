using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.UnitsOfMeasure.Dtos;
using MediatR;

namespace InventoryManagement.Application.UnitsOfMeasure.Queries.GetUnitsOfMeasure;

public sealed class GetUnitsOfMeasureQueryHandler : IRequestHandler<GetUnitsOfMeasureQuery, IReadOnlyList<UnitOfMeasureDto>>
{
    private readonly IUnitOfMeasureQueries _unitOfMeasureQueries;

    public GetUnitsOfMeasureQueryHandler(IUnitOfMeasureQueries unitOfMeasureQueries)
    {
        _unitOfMeasureQueries = unitOfMeasureQueries;
    }

    public Task<IReadOnlyList<UnitOfMeasureDto>> Handle(GetUnitsOfMeasureQuery request, CancellationToken cancellationToken)
    {
        return _unitOfMeasureQueries.GetUnitsOfMeasureAsync(cancellationToken);
    }
}
