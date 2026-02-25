using Xunit;

public class ProductRepositoryTests
{
    [Fact]
    public void Insert_Product_ShouldStoreProduct()
    {
        var repo = new ProductRepository();

        var p = new Product
        {
            Id="1",
            Name="Mouse",
            Stock=10
        };

        repo.Insert(p);

        var result = repo.Get("1");

        Assert.NotNull(result);
        Assert.Equal(10,result.Stock);
    }

    [Fact]
    public void UpdateStock_ShouldIncreaseStock()
    {
        var repo = new ProductRepository();

        repo.Insert(new Product
        {
            Id="1",
            Name="Mouse",
            Stock=10
        });

        var ok = repo.UpdateStock("1",5);

        Assert.True(ok);

        var p = repo.Get("1");

        Assert.Equal(15,p.Stock);
    }

    [Fact]
    public void UpdateStock_ShouldNotAllowNegative()
    {
        var repo=new ProductRepository();

        repo.Insert(new Product
        {
            Id="1",
            Name="Mouse",
            Stock=5
        });

        var ok=repo.UpdateStock("1",-10);

        Assert.False(ok);
    }
}