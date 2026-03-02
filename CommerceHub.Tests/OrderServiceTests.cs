using CommerceHub.API.Models;
using CommerceHub.API.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task Checkout_ShouldCreateOrder()
    {
        var orderRepo = new FakeOrderRepository();
        var productRepo = new FakeProductRepository();
        var publisher = new RabbitPublisherFake();

        await productRepo.InsertAsync(new Product
        {
            Id = "1",
            Stock = 10,
            Name = "Mouse",
            Price = 50
        });

        var service = new OrderService(orderRepo, productRepo, publisher);

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = "1", Quantity = 2 }
            }
        };

        var result = await service.CheckoutAsync(order);

        Assert.NotNull(result.Id);
        Assert.Single(publisher.Published);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenMissing()
    {
        var service = new OrderService(
            new FakeOrderRepository(),
            new FakeProductRepository(),
            new RabbitPublisherFake());

        var result = await service.GetAsync("999");

        Assert.Null(result);
    }

    [Fact]
    public async Task Checkout_ShouldFail_WhenProductMissing()
    {
        var service = new OrderService(
            new FakeOrderRepository(),
            new FakeProductRepository(),
            new RabbitPublisherFake());

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = "999", Quantity = 1 }
            }
        };

        await Assert.ThrowsAsync<Exception>(() =>
            service.CheckoutAsync(order));
    }

    [Fact]
    public async Task Checkout_ShouldFail_WhenStockInsufficient()
    {
        var orderRepo = new FakeOrderRepository();
        var productRepo = new FakeProductRepository();
        var publisher = new RabbitPublisherFake();

        await productRepo.InsertAsync(new Product
        {
            Id = "1",
            Stock = 2,
            Name = "Mouse",
            Price = 50
        });

        var service = new OrderService(orderRepo, productRepo, publisher);

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = "1", Quantity = 5 }
            }
        };

        await Assert.ThrowsAsync<Exception>(() =>
            service.CheckoutAsync(order));
    }

    [Fact]
    public async Task Checkout_ShouldReduceStock()
    {
        var orderRepo = new FakeOrderRepository();
        var productRepo = new FakeProductRepository();
        var publisher = new RabbitPublisherFake();

        await productRepo.InsertAsync(new Product
        {
            Id = "1",
            Stock = 10,
            Name = "Mouse",
            Price = 50
        });

        var service = new OrderService(orderRepo, productRepo, publisher);

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = "1", Quantity = 3 }
            }
        };

        await service.CheckoutAsync(order);

        var product = await productRepo.GetAsync("1");

        Assert.Equal(7, product.Stock);
    }

    [Fact]
    public async Task Checkout_ShouldPublishEvent()
    {
        var orderRepo = new FakeOrderRepository();
        var productRepo = new FakeProductRepository();
        var publisher = new RabbitPublisherFake();

        await productRepo.InsertAsync(new Product
        {
            Id = "1",
            Stock = 10,
            Name = "Mouse",
            Price = 50
        });

        var service = new OrderService(orderRepo, productRepo, publisher);

        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = "1", Quantity = 1 }
            }
        };

        await service.CheckoutAsync(order);

        Assert.Single(publisher.Published);
    }

    [Fact]
    public async Task Update_ShouldNotAllowChangingId()
    {
        var orderRepo = new FakeOrderRepository();
        var productRepo = new FakeProductRepository();
        var publisher = new RabbitPublisherFake();

        var service = new OrderService(orderRepo, productRepo, publisher);

        await orderRepo.InsertAsync(new Order
        {
            Id = "1",
            Status = "Pending"
        });

        var updated = new Order
        {
            Id = "999",
            Status = "Pending"
        };

        await service.UpdateAsync("1", updated);

        var saved = await orderRepo.GetAsync("1");

        Assert.Equal("1", saved.Id);
    }
}