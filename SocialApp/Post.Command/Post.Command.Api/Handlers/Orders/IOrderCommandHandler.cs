using Post.Command.Api.Commands.Orders;

namespace Post.Command.Api.Handlers.Orders;

public interface IOrderCommandHandler
{
    Task HandleAsync(CreateDiscountCommand command);
    Task HandleAsync(UpdateOrderCommand command);
    Task HandleAsync(DeleteOrderCommand command);
}