using Samples.Domain;

namespace Samples.Challenge1;

public interface IOrderRepository
{
    Task<Order?> Get(Guid id);
    Task Create(Order order);
    Task Save(Order order);
}