namespace Samples.Scheduling;

public class MeasurementsSimulatorSaga(IDatetimeProvider dateTimeProvider)
    : Saga<MeasurementsSimulatorSaga.SagaState>,
        IAmStartedByMessages<ApplicationStarted>,
        IHandleTimeouts<TimeoutTriggeredV1>
{
    public const int IntervalInSeconds = 5;
    
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaState> mapper)
    {
        mapper.MapSaga(messageProperty => messageProperty.AggregateId)
            .ToMessage<ApplicationStarted>(messageProperty => messageProperty.AggregateId)
            .ToMessage<TimeoutTriggeredV1>(messageProperty => messageProperty.AggregateId);
    }
    
    public Task Handle(ApplicationStarted message, IMessageHandlerContext context)
    {
        Data.Started = true;
        return Task.CompletedTask;
    }

    public async Task Timeout(TimeoutTriggeredV1 state, IMessageHandlerContext context)
    {
        await context.Send(new SendSimulatedMeasurements(
            "meter-1", 
            123L, 
            dateTimeProvider.Now()) // TODO: susceptible to drifting
        );

        await RequestTimeout(context,
            TimeSpan.FromSeconds(IntervalInSeconds), // TODO: susceptible to drifting
            new TimeoutTriggeredV1(Data.AggregateId));
    }
    
    public class SagaState : ContainSagaData
    {
        public string AggregateId { get; set; }
        public bool Started { get; set; }
    }

}