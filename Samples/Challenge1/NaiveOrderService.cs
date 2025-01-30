using Samples.Domain;

namespace Samples.Challenge1;

public class NaiveOrderService(IOrderRepository repository) : IOrderService
{
    
    public async Task Handle(PlaceOrderCommand message, IMessageHandlerContext context)
    {
        var order = new Order(Guid.NewGuid(), message.ProductId, message.Amount);
        
        await repository.Create(order);
    }

    public async Task Handle(OrderPaidEvent message, IMessageHandlerContext context)
    {
        var order = await repository.Get(message.OrderId);

        if (order == null)
        {
            throw new OrderNotFoundException($"Order with id {message.OrderId} does not exist");
        }

        order.MarkAsPaid();
        
        await repository.Save(order);
    }
}