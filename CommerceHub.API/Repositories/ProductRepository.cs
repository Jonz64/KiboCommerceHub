using CommerceHub.API.Models;
using MongoDB.Driver;

namespace CommerceHub.API.Data;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Product>("Products");
    }

    public async Task<Product?> GetAsync(string id)
    {
        return await _collection
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task InsertAsync(Product product)
    {
        await _collection.InsertOneAsync(product);
    }

    /// <summary>
    /// Atomically adjust stock using MongoDB atomic update
    /// </summary>
    public async Task<bool> UpdateStockAsync(string id, int amt)
    {
        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq(p => p.Id, id),
            Builders<Product>.Filter.Gte(p => p.Stock, -amt)
        );

        var update = Builders<Product>.Update.Inc(p => p.Stock, amt);

        var result = await _collection.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }
}