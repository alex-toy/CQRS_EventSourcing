using Post.Command.Api.Commands.Deliveries.Orders;

namespace Post.Command.Api.Handlers.Deliveries.Orders;

public interface IDeliveryOrderCommandHandler
{
    Task HandleAsync(AddOrderCommand command);
    Task HandleAsync(RemoveOrderCommand command);
}