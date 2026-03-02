public class RabbitPublisherFake : IRabbitPublisher
{
    public List<Order> Published { get; } = new();

    public void Publish(Order order)
    {
        Published.Add(order);
    }
}