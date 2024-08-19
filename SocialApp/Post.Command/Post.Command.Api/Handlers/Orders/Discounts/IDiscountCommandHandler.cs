using Post.Command.Api.Commands.Discounts;

namespace Post.Command.Api.Handlers.Discounts;

public interface IDiscountCommandHandler
{
    Task HandleAsync(CreateDiscountCommand command);
    Task HandleAsync(UpdateDiscountCommand command);
    Task HandleAsync(DeleteDiscountCommand command);
}