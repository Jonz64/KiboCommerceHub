using CommerceHub.API.Models;
using CommerceHub.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    // GET /api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var product = await _service.GetAsync(id);

        if (product == null)
            return NotFound(new { message = $"Product '{id}' not found." });

        return Ok(product);
    }

    // POST /api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (product == null)
            return BadRequest("Product body is required.");

        if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest("Product name is required.");

        if (product.Price <= 0)
            return BadRequest("Price must be greater than zero.");

        if (product.Stock < 0)
            return BadRequest("Stock cannot be negative.");

        await _service.InsertAsync(product);

        return CreatedAtAction(
            nameof(Get),
            new { id = product.Id },
            product);
    }

    // PATCH /api/products/{id}/stock?amount=5
    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> UpdateStock(string id, [FromQuery] int amount)
    {
        if (amount == 0)
            return BadRequest("Stock adjustment amount cannot be zero.");

        var success = await _service.AdjustStockAsync(id, amount);

        if (!success)
            return Conflict("Stock update failed. Product not found or insufficient stock.");

        return NoContent();
    }
}