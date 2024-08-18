using CQRS.Core.Events;
using Post.Command.Api.Commands.Orders.Items;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Orders.Items;

public class ItemCommandHandler : IItemCommandHandler
{
    private readonly IEventSourcingHandler<OrderAggregate> _eventSourcingHandler;

    public ItemCommandHandler(IEventSourcingHandler<OrderAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreateItemCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.AddItem(command.Label, command.Price);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateItemCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditItem(command.ItemId, command.Label, command.Price);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeleteItemCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemoveItem(command.ItemId);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}