using Moq;
using NServiceBus.Testing;
using Samples.Scheduling;
using Shouldly;

namespace SamplesTests.Scheduling;

public class MeasurementsSimulatorSagaTests
{
    private const string AggregateId = "measurements-processor-backend";
    
    private readonly DateTime Now = DateTime.Now;
    private readonly Mock<IDatetimeProvider> _datetimeProvider = new();
    private readonly MeasurementsSimulatorSaga _systemUnderTest;
    private readonly TestableMessageHandlerContext _context = new();
    
    public MeasurementsSimulatorSagaTests()
    {
        _datetimeProvider.Setup(provider => provider.Now()).Returns(Now);
        
        _systemUnderTest = new MeasurementsSimulatorSaga(_datetimeProvider.Object);
        _systemUnderTest.Data = new MeasurementsSimulatorSaga.SagaState
        {
            AggregateId = AggregateId
        };
    }
    
    [Fact]
    public async Task TimeoutV1_SendsSendSimulatedMeasurements()
    {
        // Arrange
        var timeout = new TimeoutTriggered(AggregateId);
        
        // Act
        await _systemUnderTest.Timeout(timeout, _context);
        
        // Assert
        var actual = _context.FindSentMessage<SendSimulatedMeasurements>();
        actual.ShouldNotBeNull();
    }

    [Fact]
    public async Task TimeoutV1_SchedulesNextTimeout()
    {
        // Arrange
        var timeout = new TimeoutTriggered(AggregateId);
        
        // Act
        await _systemUnderTest.Timeout(timeout, _context);
        
        // Assert
        var actual = _context.FindSentMessage<TimeoutTriggered>();
        actual.ShouldNotBeNull();
    }

    // https://docs.particular.net/nservicebus/testing/saga-scenario-testing
    [Fact]
    public async Task Saga_AfterMultipleIntervalsOfDowntime_SendsCommandForEveryInterval()
    {
        // Arrange
        var expectedAmountOfCommands = 3;
        var systemUnderTest = new TestableSaga<MeasurementsSimulatorSaga, MeasurementsSimulatorSaga.SagaState>(
                () => new MeasurementsSimulatorSaga(_datetimeProvider.Object), 
                Now
        );

        var applicationStarted = new ApplicationStarted(AggregateId);
        await systemUnderTest.Handle(applicationStarted);
        
        // Act (simulate downtime by advancing more than <interval>)
        var result = await systemUnderTest
            .AdvanceTime(expectedAmountOfCommands * TimeSpan.FromSeconds(MeasurementsSimulatorSaga.IntervalInSeconds));
        
        // Assert
        result.Length.ShouldBe(1); // One timeout has triggered. We had ~ 3 intervals of downtime
        var sentCommands
            = result[0].Context.SentMessages
                .Select(m => m.Message)
                .OfType<SendSimulatedMeasurements>()
                .ToList();
        sentCommands.Count.ShouldBe(expectedAmountOfCommands);
    }
}