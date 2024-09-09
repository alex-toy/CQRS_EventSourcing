using Post.Command.Api.Commands.Deliveries;

namespace Post.Command.Api.Handlers.Deliveries;

public interface IDeliveryCommandHandler
{
    Task HandleAsync(CreateDeliveryCommand command);
    Task HandleAsync(UpdateDeliveryCommand command);
    //Task HandleAsync(AddOrderCommand command);
    //Task HandleAsync(LikePostCommand command);
    //Task HandleAsync(DeletePostCommand command);
}