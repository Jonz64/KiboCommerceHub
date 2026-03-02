using CommerceHub.API.Data;
using CommerceHub.API.Models;

namespace CommerceHub.API.Services;

public class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Product?> GetAsync(string id)
    {
        return await _repo.GetAsync(id);
    }

    public async Task<bool> AdjustStockAsync(string id, int amt)
    {
        return await _repo.UpdateStockAsync(id, amt);
    }

    public async Task InsertAsync(Product p)
    {
        p.Id = Guid.NewGuid().ToString();
        await _repo.InsertAsync(p);
    }
}