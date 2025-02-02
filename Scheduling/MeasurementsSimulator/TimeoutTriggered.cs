namespace Samples.Scheduling;

public record TimeoutTriggered(string AggregateId) : IEvent;