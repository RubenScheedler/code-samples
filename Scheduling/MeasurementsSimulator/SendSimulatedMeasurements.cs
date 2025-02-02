namespace Samples.MeasurementsSimulator;

public record SendSimulatedMeasurements(string Meter, long MeasuredValue, DateTime MeasuredAt) : ICommand;