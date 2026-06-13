using InventoryManagement.Api.Contracts;
using InventoryManagement.Application.Currencies.Commands.UpdateCurrencyStatus;
using InventoryManagement.Application.Currencies.Dtos;
using InventoryManagement.Application.Currencies.Queries.GetCurrencies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CurrencyDto>>> Get(CancellationToken cancellationToken)
    {
        var currencies = await _mediator.Send(new GetCurrenciesQuery(), cancellationToken);
        return Ok(currencies);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new UpdateCurrencyStatusCommand(id, request.Status), cancellationToken);
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
