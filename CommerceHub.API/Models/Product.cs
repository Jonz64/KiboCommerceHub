using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommerceHub.API.Models;

public class Product
{
    [BsonId]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}