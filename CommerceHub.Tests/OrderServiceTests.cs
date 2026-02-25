public class OrderServiceTests
{
    [Fact]
    public void Checkout_ShouldCreateOrder()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();

        productRepo.Insert(new Product
        {
            Id="1",
            Stock=10
        });

        var pub = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            pub);

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId="1",
                    Quantity=2
                }
            }
        };

        var result = service.Checkout(order);

        Assert.NotNull(result.Id);
    }

    [Fact]
    public void Get_ShouldReturnOrder_WhenExists()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            publisher);

        var order = new Order
        {
            Id = "1",
            Status = "Pending"
        };

        orderRepo.Insert(order);

        var result = service.Get("1");

        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }


    [Fact]
    public void Get_ShouldReturnNull_WhenMissing()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            publisher);

        var result = service.Get("999");

        Assert.Null(result);
    }

    [Fact]
    public void Update_ShouldWork_WhenOrderExists()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            publisher);

        var order = new Order
        {
            Id = "1",
            Status = "Pending"
        };

        orderRepo.Insert(order);

        var updated = new Order
        {
            Status = "Pending"
        };

        var result = service.Update("1", updated);

        Assert.True(result);

        var saved = orderRepo.Get("1");

        Assert.Equal("Pending", saved.Status);
    }



    [Fact]
    public void Update_ShouldFail_WhenMissing()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            publisher);

        var order = new Order();

        var result = service.Update("999", order);

        Assert.False(result);
    }



    [Fact]
    public void Update_ShouldFail_WhenShipped()
    {
        var orderRepo = new OrderRepository();
        var productRepo = new ProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(
            orderRepo,
            productRepo,
            publisher);

        var order = new Order
        {
            Id="1",
            Status="Shipped"
        };

        orderRepo.Insert(order);

        var updated = new Order
        {
            Status="Pending"
        };

        var result = service.Update("1", updated);

        Assert.False(result);
    }
}