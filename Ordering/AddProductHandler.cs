namespace Ordering;

public record AddProduct(Guid OrderId, Guid ProductId);

public class AddProductHandler(IOrderRepository repository) : IHandleMessages<AddProduct>
{
    public async Task Handle(AddProduct command, IMessageHandlerContext context)
    {
        var order = await repository.Get(command.OrderId);
        
        order.AddProduct(command.ProductId);

        await repository.Save(order);
    }
}

