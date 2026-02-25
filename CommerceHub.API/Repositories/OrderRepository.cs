
public class OrderRepository
{
    private Dictionary<string,Order> db = new();

    public Order? Get(string id)
    {
        return db.ContainsKey(id) ? db[id] : null;
    }

    public void Insert(Order o)
    {
        db[o.Id]=o;
    }

    public bool Update(string id, Order o)
    {
        if (!db.ContainsKey(id))
            return false;

        var existing = db[id];

        // Block updates if order already shipped
        if (existing.Status == "Shipped")
            return false;

        db[id] = o;
        return true;
    }
}
