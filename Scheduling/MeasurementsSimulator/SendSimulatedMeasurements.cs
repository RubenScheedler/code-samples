namespace Samples.MeasurementsSimulator;

public record SendSimulatedMeasurements(string Sensor, long MeasuredValue, DateTime MeasuredAt) : ICommand;