
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

public class ProductService
{
    ProductRepository repo;

    public ProductService(ProductRepository r)
    {
        repo=r;
    }

    public bool AdjustStock(string id,int amt)
    {
        return repo.UpdateStock(id,amt);
    }

    public void Insert(Product p)
    {
        p.Id = Guid.NewGuid().ToString();
        repo.Insert(p);
    }
}
