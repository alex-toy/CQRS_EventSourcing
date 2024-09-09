using Post.Command.Api.Commands.Deliveries;

namespace Post.Command.Api.Handlers.Deliveries;

public interface IDeliveryCommandHandler
{
    Task HandleAsync(CreateDeliveryCommand command);
    Task HandleAsync(UpdateDeliveryCommand command);
    Task HandleAsync(DeleteDeliveryCommand command);
}