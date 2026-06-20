using InventoryManagement.Api.Contracts;
using InventoryManagement.Application.Products.Commands.CreateProduct;
using InventoryManagement.Application.Products.Commands.UpdateProduct;
using InventoryManagement.Application.Products.Commands.UpdateProductStatus;
using InventoryManagement.Application.Products.Dtos;
using InventoryManagement.Application.Products.Queries.GetInventoryValueByCategory;
using InventoryManagement.Application.Products.Queries.GetLowStockProducts;
using InventoryManagement.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var productId = await _mediator.Send(new CreateProductCommand(
            request.Name,
            request.Barcode,
            request.Description,
            request.CurrencyCode,
            request.PurchasePrice,
            request.SalePrice,
            request.Stock,
            request.MinimumStock,
            request.Status,
            request.CategoryId,
            request.UnitOfMeasureId), cancellationToken);

        return Ok(new { id = productId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new UpdateProductCommand(
                id,
                request.Name,
                request.Barcode,
                request.Description,
                request.CurrencyCode,
                request.PurchasePrice,
                request.SalePrice,
                request.Stock,
                request.MinimumStock,
                request.Status,
                request.CategoryId,
                request.UnitOfMeasureId), cancellationToken);

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

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new UpdateProductStatusCommand(id, request.Status), cancellationToken);
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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> Get(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetLowStock(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("inventory-value-by-category")]
    public async Task<ActionResult<IReadOnlyList<InventoryValueByCategoryDto>>> GetInventoryValueByCategory(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetInventoryValueByCategoryQuery(), cancellationToken);
        return Ok(response);
    }
}
