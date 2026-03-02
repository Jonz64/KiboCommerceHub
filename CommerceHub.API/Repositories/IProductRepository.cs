using CommerceHub.API.Models;

public interface IProductRepository
{
    Task<Product?> GetAsync(string id);
    Task InsertAsync(Product product);
    Task<bool> UpdateStockAsync(string id, int amount);
}