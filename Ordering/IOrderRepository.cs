namespace Ordering;

public interface IOrderRepository
{
    Task<Order> Get(Guid id);
    Task Save(Order order);
}