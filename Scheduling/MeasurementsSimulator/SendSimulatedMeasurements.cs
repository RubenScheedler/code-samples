namespace Samples.Scheduling;

public record SendSimulatedMeasurements(string Meter, long MeasuredValue, DateTime MeasuredAt) : ICommand;