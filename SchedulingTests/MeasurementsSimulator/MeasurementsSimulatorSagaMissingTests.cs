using System.Text.Json;

namespace SamplesTests.MeasurementsSimulator;

using NServiceBus.Testing;
using Samples.MeasurementsSimulator;
using Shouldly;

public partial class MeasurementsSimulatorSagaTests
{
    [Fact]
    public async Task Timeout_OldExistingMessageWithoutTriggeredAt_YieldsOneMessage()
    {
        // Arrange
        var timeout = JsonSerializer.Deserialize<TimeoutTriggered>("{\"AggregateId\": \""+AggregateId+"\"}");
        
        // Act
        await _systemUnderTest.Timeout(timeout!, _context);
        
        // Assert
        var actual = _context.FindSentMessage<SendSimulatedMeasurements>();
        actual.ShouldNotBeNull();
    }
}