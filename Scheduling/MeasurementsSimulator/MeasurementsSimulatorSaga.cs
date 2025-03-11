namespace Samples.MeasurementsSimulator;

public class MeasurementsSimulatorSaga(IDatetimeProvider dateTimeProvider)
    : Saga<MeasurementsSimulatorSaga.SagaState>,
        IAmStartedByMessages<SimulationStarted>,
        IHandleTimeouts<TimeoutTriggered>
{
    public const int IntervalInSeconds = 3600;
    
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
            TimeSpan.FromSeconds(IntervalInSeconds),
            new TimeoutTriggered(Data.AggregateId)
        );
    }

    public async Task Timeout(TimeoutTriggered timeoutMessage, IMessageHandlerContext context)
    {
        await context.Send(new SendSimulatedMeasurements(
            "sensor-1", 
            GenerateStubTemperatureValue(), 
            dateTimeProvider.Now())
        );

        await RequestTimeout(context,
            TimeSpan.FromSeconds(IntervalInSeconds),
            new TimeoutTriggered(Data.AggregateId));
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