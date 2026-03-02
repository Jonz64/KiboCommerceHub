using CommerceHub.API.Models;
using CommerceHub.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    // POST /api/orders/checkout
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] Order order)
    {
        if (order == null || order.Items.Count == 0)
            return BadRequest("Order must contain at least one item.");

        try
        {
            var createdOrder = await _service.CheckoutAsync(order);

            return CreatedAtAction(
                nameof(Get),
                new { id = createdOrder.Id },
                createdOrder);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // e.g., insufficient stock
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET /api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var order = await _service.GetAsync(id);

        if (order == null)
            return NotFound(new { message = $"Order '{id}' not found." });

        return Ok(order);
    }

    // PUT /api/orders/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Order updatedOrder)
    {
        if (updatedOrder == null)
            return BadRequest("Order body is required.");

        var success = await _service.UpdateAsync(id, updatedOrder);

        if (!success)
            return NotFound(new { message = $"Order '{id}' not found or cannot be updated." });

        return NoContent();
    }
}