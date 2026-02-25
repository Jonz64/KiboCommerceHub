public class ProductRepository
{
    private readonly Dictionary<string, Product> db = new();
    private readonly object _lock = new(); // global lock for simplicity

    public Product? Get(string id)
    {
        lock (_lock) // safe read
        {
            return db.ContainsKey(id) ? db[id] : null;
        }
    }

    /// <summary>
    /// Atomically adjust stock. Returns false if product doesn't exist or stock would go negative.
    /// </summary>
    public bool UpdateStock(string id, int amt)
    {
        lock (_lock) // ensures atomicity
        {
            if (!db.ContainsKey(id))
                return false;

            var p = db[id];

            if (p.Stock + amt < 0)
                return false;

            p.Stock += amt;
            return true;
        }
    }

    public void Insert(Product p)
    {
        lock (_lock)
        {
            db[p.Id] = p;
        }
    }
}