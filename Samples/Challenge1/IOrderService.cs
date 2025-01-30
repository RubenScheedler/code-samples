namespace Samples.Challenge1;

public interface IOrderService : IHandleMessages<PlaceOrderCommand>, IHandleMessages<OrderPaidEvent>;