namespace Ordering;

public interface IReservationRepository
{
    Task<Reservation> Get(Guid id);
    Task Save(Reservation reservation);
}