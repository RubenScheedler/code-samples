namespace Samples.MeasurementsSimulator;

public class MeasurementsSimulatorSaga(IDatetimeProvider dateTimeProvider)
    : Saga<MeasurementsSimulatorSaga.SagaState>,
        IAmStartedByMessages<ApplicationStarted>,
        IHandleTimeouts<TimeoutTriggered>
{
    public const int IntervalInSeconds = 5;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(IntervalInSeconds);
    
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaState> mapper)
    {
        mapper.MapSaga(messageProperty => messageProperty.AggregateId)
            .ToMessage<ApplicationStarted>(messageProperty => messageProperty.AggregateId)
            .ToMessage<TimeoutTriggered>(messageProperty => messageProperty.AggregateId);
    }
    
    public async Task Handle(ApplicationStarted message, IMessageHandlerContext context)
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
                "meter-1", 
                ++Data.LastMeasuredValue, 
                measuredAt)
            ));

            measuredAt = measuredAt.Add(_interval);
        }

        await Task.WhenAll(measurementsTasks);
        
        await RequestTimeout(context,
            _interval, 
            new TimeoutTriggered(Data.AggregateId, dateTimeProvider.Now()));

    }
    
    public class SagaState : ContainSagaData
    {
        public string AggregateId { get; set; } = null!;
        public long LastMeasuredValue { get; set; }
    }

}