namespace Samples.Scheduling;

public class MeasurementsSimulatorSaga(IDatetimeProvider dateTimeProvider)
    : Saga<MeasurementsSimulatorSaga.SagaState>,
        IAmStartedByMessages<ApplicationStarted>,
        IHandleTimeouts<TimeoutTriggered>
{
    public const int IntervalInSeconds = 5;
    
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
            TimeSpan.FromSeconds(IntervalInSeconds),
            new TimeoutTriggered(Data.AggregateId)
        );
    }

    public async Task Timeout(TimeoutTriggered timeoutMessage, IMessageHandlerContext context)
    {
        await context.Send(new SendSimulatedMeasurements(
            "meter-1", 
            ++Data.LastMeasuredValue, 
            dateTimeProvider.Now())
        );

        await RequestTimeout(context,
            TimeSpan.FromSeconds(IntervalInSeconds), // TODO: susceptible to drifting
            new TimeoutTriggered(Data.AggregateId));
    }
    
    public class SagaState : ContainSagaData
    {
        public string AggregateId { get; set; } = null!;
        public long LastMeasuredValue { get; set; }
    }

}