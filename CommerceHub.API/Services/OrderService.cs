
public class OrderService
{
    OrderRepository orders;
    ProductRepository products;
    IRabbitPublisher pub;

    public OrderService(
        OrderRepository o,
        ProductRepository p,
        IRabbitPublisher r)
    {
        orders=o;
        products=p;
        pub=r;
    }

    public Order Checkout(Order o)
    {
        foreach(var i in o.Items)
        {
            if(i.Quantity<=0)
                throw new Exception();
        }

        foreach(var i in o.Items)
        {
            if(!products.UpdateStock(
                i.ProductId,
                -i.Quantity))
                throw new Exception();
        }

        o.Id=Guid.NewGuid().ToString();

        orders.Insert(o);

        pub.Publish(o);

        return o;
    }

    // Get order by ID (GET /api/orders/{id})
    public Order? Get(string id)
    {
        Order order = orders.Get(id);

        return order;
    }

    // PUT /api/orders/{id}
    public bool Update(string id, Order o)
    {
        return orders.Update(id, o);
    }
}
