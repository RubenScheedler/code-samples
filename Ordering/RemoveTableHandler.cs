namespace Ordering;

public record RemoveTable(Guid ReservationId, long TableId);

public class RemoveTableHandler(IReservationRepository repository) : IHandleMessages<RemoveTable>
{
    public async Task Handle(RemoveTable command, IMessageHandlerContext context)
    {
        var order = await repository.Get(command.ReservationId);
        
        order.RemoveTable(command.TableId);

        await repository.Save(order);
    }
}




