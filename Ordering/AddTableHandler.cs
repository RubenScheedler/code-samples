namespace Ordering;

public record AddTable(Guid OrderId, long TableNumber);

public class AddTableHandler(IReservationRepository repository) : IHandleMessages<AddTable>
{
    public async Task Handle(AddTable command, IMessageHandlerContext context)
    {
        var order = await repository.Get(command.OrderId);
        
        order.AddTable(command.TableNumber);

        await repository.Save(order);
    }
}

