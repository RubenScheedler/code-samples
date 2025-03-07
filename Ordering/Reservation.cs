namespace Ordering;

public class Reservation
{
    public Guid Id { get; }
    public List<long> Tables { get; } = [];
    public bool IsCancelled { get; private set; }
    
    public void AddTable(long tableNumber)
    {
        if (IsCancelled)
        {
            // What to do?
        }
        
        if (!Tables.Contains(tableNumber))
        {
            Tables.Add(tableNumber);
        }
    }

    public void RemoveTable(long tableNumber)
    {
        if (!Tables.Contains(tableNumber))
        {
            // What to do?
        }
        
        Tables.Remove(tableNumber);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}