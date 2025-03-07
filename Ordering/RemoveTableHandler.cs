namespace Ordering;

public record RemoveTable(Guid ReservationId, long TableId);

public class RemoveTableHandler(IReservationRepository repository) : IHandleMessages<RemoveTable>
{
    public async Task Handle(RemoveTable command, IMessageHandlerContext context)
    {
        var reservation = await repository.Get(command.ReservationId);
        
        reservation.RemoveTable(command.TableId);

        await repository.Save(reservation);
    }
}




