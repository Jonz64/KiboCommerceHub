
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitPublisher : IRabbitPublisher
{
    IConnection conn;

    public RabbitPublisher()
    {
        var f=new ConnectionFactory()
        {
            HostName="rabbit"
        };

        conn=f.CreateConnection();
    }

    public void Publish(Order o)
    {
        using var ch=conn.CreateModel();

        ch.ExchangeDeclare(
            "orders.exchange",
            "fanout");

        var body=Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(o));

        Console.WriteLine(body.ToString() + "$##$#$#$#$$##$#$#$#$#$#$#$");
        ch.BasicPublish(
            "orders.exchange",
            "",
            null,
            body);
    }
}
