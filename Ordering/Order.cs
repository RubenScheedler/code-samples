namespace Ordering;

public class Order
{
    public Guid Id { get; }
    public List<Guid> Products { get; } = [];
    public bool IsCancelled { get; private set; }
    
    public void AddProduct(Guid productId)
    {
        if (IsCancelled)
        {
            // What to do?
        }
        
        if (!Products.Contains(productId))
        {
            Products.Add(productId);
        }
    }

    public void RemoveProduct(Guid productId)
    {
        if (!Products.Contains(productId))
        {
            // What to do?
        }
        
        Products.Remove(productId);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}