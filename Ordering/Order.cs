namespace Ordering;

public class Order
{
    private Guid Id { get; set; }
    public List<Guid> Products { get; } = [];
}