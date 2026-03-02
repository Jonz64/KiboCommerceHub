public interface IOrderRepository
{
    Task<Order?> GetAsync(string id);
    Task InsertAsync(Order order);
    Task<bool> UpdateAsync(string id, Order updated);
}