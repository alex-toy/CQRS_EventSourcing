﻿using CQRS.Core.Events;
using Post.Command.Api.Commands.Orders;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Orders;

public class OrderCommandHandler : IOrderCommandHandler
{
    private readonly IEventSourcingHandler<OrderAggregate> _eventSourcingHandler;

    public OrderCommandHandler(IEventSourcingHandler<OrderAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreateOrderCommand command)
    {
        OrderAggregate aggregate = new(command.Id, command.Author, command.ItemId, command.Quantity);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateOrderCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditQuantity(command.Quantity);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeleteOrderCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.DeleteOrder(command.Author);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}