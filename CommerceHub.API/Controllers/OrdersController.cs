
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class OrdersController:ControllerBase
{
    OrderService s;

    public OrdersController(OrderService x)
    {
        s=x;
    }

    [HttpPost("checkout")]
    public IActionResult Post(Order o)
    {
        return Ok(s.Checkout(o));
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(s.Get(id));
    }

    // PUT /api/orders/{id}
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] Order updatedOrder)
    {
        return Ok(s.Update(id, updatedOrder));
    }
}
