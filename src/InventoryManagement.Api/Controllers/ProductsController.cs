using InventoryManagement.Application.Products.Commands.CreateProduct;
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
