using System.Text.Json;

namespace SamplesTests.MeasurementsSimulator;

using NServiceBus.Testing;
using Samples.MeasurementsSimulator;
using Shouldly;

public partial class MeasurementsSimulatorSagaTests
{
    // hide actual name for demo: Timeout_OldExistingMessageWithoutTriggeredAt_YieldsOneMessage
    [Fact]
    public async Task MissingTest()
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