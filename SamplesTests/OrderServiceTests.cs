using Moq;
using NServiceBus.Testing;
using Samples.Challenge1;
using Samples.Domain;

namespace SamplesTests;

public class OrderServiceTests
{
    private readonly Guid _orderId = Guid.NewGuid();
    private readonly Mock<IOrderRepository> _repository = new();
    private readonly TestableMessageHandlerContext _context = new ();
    private readonly IOrderService _systemUnderTest;
    public OrderServiceTests()
    {
        // switch between different implementations to see how they hold up in the same scenarios
        _systemUnderTest = new NaiveOrderService(_repository.Object);
    }

}