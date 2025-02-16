namespace Ordering;

public record RemoveProduct(Guid OrderId, Guid ProductId);

public class RemoveProductHandler(IOrderRepository repository) : IHandleMessages<RemoveProduct>
{
    public async Task Handle(RemoveProduct command, IMessageHandlerContext context)
    {
        var order = await repository.Get(command.OrderId);
        
        order.RemoveProduct(command.ProductId);

        await repository.Save(order);
    }
}




