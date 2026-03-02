using CommerceHub.API.Models;

public class FakeProductRepository : IProductRepository
{
    private readonly Dictionary<string, Product> _products = new();

    public Task InsertAsync(Product product)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task<Product?> GetAsync(string id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<bool> UpdateStockAsync(string id, int amount)
    {
        if (!_products.ContainsKey(id))
            return Task.FromResult(false);

        var product = _products[id];

        if (product.Stock + amount < 0)
            return Task.FromResult(false);

        product.Stock += amount;
        return Task.FromResult(true);
    }
}