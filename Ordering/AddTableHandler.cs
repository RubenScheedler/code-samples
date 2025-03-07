namespace Ordering;

public record AddTable(Guid ReservationId, long TableNumber);

public class AddTableHandler(IReservationRepository repository) : IHandleMessages<AddTable>
{
    public async Task Handle(AddTable command, IMessageHandlerContext context)
    {
        var reservation = await repository.Get(command.ReservationId);
        
        reservation.AddTable(command.TableNumber);

        await repository.Save(reservation);
    }
}

