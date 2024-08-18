using Post.Command.Api.Commands.Orders.Items;

namespace Post.Command.Api.Handlers.Orders.Items;

public interface IItemCommandHandler
{
    Task HandleAsync(CreateItemCommand command);
    Task HandleAsync(UpdateItemCommand command);
    Task HandleAsync(DeleteItemCommand command);
}