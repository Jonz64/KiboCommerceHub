
public class Order
{
    public string Id { get; set; } = "";
    public string Status { get; set; } = "Pending";
    public List<OrderItem> Items { get; set; } = new();
}
