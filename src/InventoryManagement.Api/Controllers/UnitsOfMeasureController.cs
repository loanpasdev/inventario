using InventoryManagement.Api.Contracts;
using InventoryManagement.Application.UnitsOfMeasure.Commands.UpdateUnitOfMeasureStatus;
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

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new UpdateUnitOfMeasureStatusCommand(id, request.Status), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}
