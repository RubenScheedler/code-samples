namespace Samples.MeasurementsSimulator;

public class MeasurementsSimulatorSaga(IDatetimeProvider dateTimeProvider)
    : Saga<MeasurementsSimulatorSaga.SagaState>,
        IAmStartedByMessages<SimulationStarted>,
        IHandleTimeouts<TimeoutTriggered>
{
    public const int IntervalInSeconds = 3600;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(IntervalInSeconds);
    
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaState> mapper)
    {
        mapper.MapSaga(messageProperty => messageProperty.AggregateId)
            .ToMessage<SimulationStarted>(messageProperty => messageProperty.AggregateId)
            .ToMessage<TimeoutTriggered>(messageProperty => messageProperty.AggregateId);
    }
    
    public async Task Handle(SimulationStarted message, IMessageHandlerContext context)
    {
        Data.LastMeasuredValue = 0;
        await RequestTimeout(context,
            _interval,
            new TimeoutTriggered(Data.AggregateId, dateTimeProvider.Now())
        );
    }

    public async Task Timeout(TimeoutTriggered timeoutMessage, IMessageHandlerContext context)
    {
        var measuredAt = timeoutMessage.TriggeredAt;
        
        var measurementsTasks = new List<Task>();
        
        while (measuredAt <= dateTimeProvider.Now()) // Fill in downtime gaps
        {
            measurementsTasks.Add(context.Send(new SendSimulatedMeasurements(
                "sensor-1", 
                GenerateStubTemperatureValue(), 
                measuredAt)
            ));

            measuredAt = measuredAt.Add(_interval);
        }

        await Task.WhenAll(measurementsTasks);
        
        await RequestTimeout(context,
            _interval, 
            new TimeoutTriggered(Data.AggregateId, dateTimeProvider.Now()));

    }

    #region Stub value calculations
    private long GenerateStubTemperatureValue()
    {
        return ++Data.LastMeasuredValue;
    }
    
    #endregion

    public class SagaState : ContainSagaData
    {
        public string AggregateId { get; set; } = null!;
        public long LastMeasuredValue { get; set; }
    }

}