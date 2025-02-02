namespace Samples.MeasurementsSimulator;

public record TimeoutTriggered(string AggregateId) : IEvent;