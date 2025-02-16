namespace Ordering;

public class Order
{
    private Guid Id { get; set; }
    public List<Guid> Products { get; } = [];

    public void AddProduct(Guid productId)
    {
        if (!Products.Contains(productId))
        {
            Products.Add(productId);
        }
    }

    public void RemoveProduct(Guid productId)
    {
        if (!Products.Contains(productId))
        {
            throw new InvalidOperationException();
        }
        
        Products.Remove(productId);
    }
}