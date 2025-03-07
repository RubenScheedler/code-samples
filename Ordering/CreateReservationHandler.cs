namespace Ordering;

public record CreateReservation(Guid Id);

public class CreateReservationHandler(IReservationRepository repository) : IHandleMessages<CreateReservation>
{
    public async Task Handle(CreateReservation command, IMessageHandlerContext context)
    {
        var existingReservation = await repository.Get(command.Id);

        if (existingReservation is not null)
        {
            // TODO what do we do?
        }

        await repository.Save(existingReservation);
    }
}

