namespace Samples.MeasurementsSimulator;

public record TimeoutTriggered(string AggregateId, DateTime TriggeredAt) : IEvent;