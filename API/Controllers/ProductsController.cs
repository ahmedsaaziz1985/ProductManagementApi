using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Application.Features.Products.Commands.CreateProduct;
using ProductManagementApi.Application.Features.Products.Commands.DeleteProduct;
using ProductManagementApi.Application.Features.Products.Commands.UpdateProduct;
using ProductManagementApi.Application.Features.Products.DTOs;
using ProductManagementApi.Application.Features.Products.Queries.GetProductById;
using ProductManagementApi.Application.Features.Products.Queries.GetProducts;
using ProductManagementApi.Application.Features.Products.Queries.SearchProducts;

namespace ProductManagementApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetProductsQuery(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new SearchProductsQuery(search, pageNumber, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateProductCommand(dto.Name, dto.Description, dto.Price, dto.Stock, dto.Currency),
            cancellationToken);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductDto dto,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateProductCommand(id, dto.Name, dto.Description, dto.Price, dto.Stock, dto.Currency),
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
