public class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<string, Order> _orders = new();

    public Task InsertAsync(Order order)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> GetAsync(string id)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task<bool> UpdateAsync(string id, Order updated)
    {
        if (!_orders.ContainsKey(id))
            return Task.FromResult(false);

        if (_orders[id].Status == "Shipped")
            return Task.FromResult(false);

        updated.Id = id;
        _orders[id] = updated;
        return Task.FromResult(true);
    }
}