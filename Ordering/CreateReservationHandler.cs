namespace Ordering;

public record CreateReservation(Guid Id);

public class CreateReservationHandler : IHandleMessages<CreateReservation>
{
    public async Task Handle(CreateReservation message, IMessageHandlerContext context)
    {
        throw new NotImplementedException();
    }
}