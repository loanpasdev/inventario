using InventoryManagement.Application.UnitsOfMeasure.Dtos;
using InventoryManagement.Application.UnitsOfMeasure.Queries.GetUnitsOfMeasure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/units-of-measure")]
public sealed class UnitsOfMeasureController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitsOfMeasureController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UnitOfMeasureDto>>> Get(CancellationToken cancellationToken)
    {
        var unitsOfMeasure = await _mediator.Send(new GetUnitsOfMeasureQuery(), cancellationToken);
        return Ok(unitsOfMeasure);
    }
}
