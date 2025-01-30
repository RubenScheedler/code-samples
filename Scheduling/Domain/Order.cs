namespace Samples.Domain;

public class Order(Guid id, long productId, int amount)
{
    public Guid Id { get; set; } = id;
    public long ProductId { get; set; } = productId;
    public int Amount { get; set; } = amount;
    public bool Paid { get; private set; }
    

    public void MarkAsPaid()
    {
        Paid = true;
    }
}