namespace Ordering;

public record CreateOrder(Guid Id);

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    public async Task Handle(CreateOrder message, IMessageHandlerContext context)
    {
        throw new NotImplementedException();
    }
}