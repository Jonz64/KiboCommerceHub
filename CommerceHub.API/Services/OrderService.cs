using CommerceHub.API.Data;
using CommerceHub.API.Models;

namespace CommerceHub.API.Services;

public class OrderService
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IRabbitPublisher _publisher;

    public OrderService(
        IOrderRepository orders,
        IProductRepository products,
        IRabbitPublisher publisher)
    {
        _orders = orders;
        _products = products;
        _publisher = publisher;
    }

    public async Task<Order> CheckoutAsync(Order o)
    {
        // Validate quantities
        foreach (var item in o.Items)
        {
            if (item.Quantity <= 0)
                throw new Exception("Invalid quantity.");
        }

        // Atomically decrement stock
        foreach (var item in o.Items)
        {
            var success = await _products.UpdateStockAsync(
                item.ProductId,
                -item.Quantity);

            if (!success)
                throw new Exception("Insufficient stock.");
        }

        o.Id = Guid.NewGuid().ToString();

        await _orders.InsertAsync(o);

        // Publish event (can remain sync or async depending on your implementation)
        _publisher.Publish(o);

        return o;
    }

    public async Task<Order?> GetAsync(string id)
    {
        return await _orders.GetAsync(id);
    }

    public async Task<bool> UpdateAsync(string id, Order o)
    {
        return await _orders.UpdateAsync(id, o);
    }
}