using CommerceHub.API.Models;
using MongoDB.Driver;

namespace CommerceHub.API.Data;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _collection;

    public OrderRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Order>("Orders");
    }

    public async Task<Order?> GetAsync(string id)
    {
        return await _collection
            .Find(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task InsertAsync(Order order)
    {
        await _collection.InsertOneAsync(order);
    }

    public async Task<bool> UpdateAsync(string id, Order updated)
    {
        // Only update if:
        // 1. Order exists
        // 2. Status is NOT "Shipped"

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Id, id),
            Builders<Order>.Filter.Ne(o => o.Status, "Shipped")
        );

        var result = await _collection.ReplaceOneAsync(filter, updated);

        return result.ModifiedCount > 0;
    }
}