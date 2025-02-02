using Moq;
using NServiceBus.Testing;
using Samples.MeasurementsSimulator;
using Shouldly;

namespace SamplesTests.MeasurementsSimulator;

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
        var timeout = new TimeoutTriggered(AggregateId, Now);
        
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
        var timeout = new TimeoutTriggered(AggregateId, Now);
        
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
        var systemUnderTest = new TestableSaga<MeasurementsSimulatorSaga, MeasurementsSimulatorSaga.SagaState>(
            () => new MeasurementsSimulatorSaga(_datetimeProvider.Object), 
            Now
        );
        
        // Start the timeout loop
        var simulationStarted = new SimulationStarted(AggregateId);
        await systemUnderTest.Handle(simulationStarted);
        
        // Skip 3 intervals into the future
        _datetimeProvider.Setup(provider => provider.Now())
            .Returns(Now.AddSeconds(3 * MeasurementsSimulatorSaga.IntervalInSeconds));
        
        // Act
        // Trigger timeout message
        var result = await systemUnderTest
            .AdvanceTime(TimeSpan.FromSeconds(MeasurementsSimulatorSaga.IntervalInSeconds));
        
        // Assert
        result.Length.ShouldBe(1); // One timeout has triggered, even though 3 intervals passed
        var sentCommands
            = result[0].Context.SentMessages
                .Select(m => m.Message)
                .OfType<SendSimulatedMeasurements>()
                .ToList();
        sentCommands.Count.ShouldBe(4); // 1 + 3 to fill the gaps
    }
}