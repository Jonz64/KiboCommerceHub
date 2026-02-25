
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController:ControllerBase
{
    ProductService s;

    public ProductsController(ProductService x)
    {
        s=x;
    }

    [HttpPatch("{id}/stock")]
    public IActionResult Patch(
        string id,
        int amt)
    {
        if(!s.AdjustStock(id,amt))
            return BadRequest();

        return Ok();
    }

   [HttpPost]
    public IActionResult Create(Product p)
    {
        if(p.Id == null || p.Id == "")
            return BadRequest();

        s.Insert(p);

        return Ok(p);
    }
}
