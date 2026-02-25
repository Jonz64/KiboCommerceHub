using Xunit;

public class OrderRepositoryTests
{
    [Fact]
    public void Insert_Order_ShouldStoreOrder()
    {
        var repo=new OrderRepository();

        var order=new Order
        {
            Id="1"
        };

        repo.Insert(order);

        var result=repo.Get("1");

        Assert.NotNull(result);
    }

    [Fact]
    public void Update_Order_ShouldReplaceOrder()
    {
        var repo=new OrderRepository();

        var order=new Order
        {
            Id="1"
        };

        repo.Insert(order);

        var newOrder=new Order
        {
            Id="1"
        };

        var ok=repo.Update("1",newOrder);

        Assert.True(ok);
    }

    [Fact]
    public void Get_MissingOrder_ShouldReturnNull()
    {
        var repo=new OrderRepository();

        var result=repo.Get("999");

        Assert.Null(result);
    }
}