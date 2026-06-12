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
}
