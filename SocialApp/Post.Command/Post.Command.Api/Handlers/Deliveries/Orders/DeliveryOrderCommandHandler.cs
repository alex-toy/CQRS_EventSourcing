using CQRS.Core.Events;
using Post.Command.Api.Commands.Deliveries.Orders;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Deliveries.Orders;

public class DeliveryOrderCommandHandler : IDeliveryOrderCommandHandler
{
    private readonly IEventSourcingHandler<DeliveryAggregate> _eventSourcingHandler;

    public DeliveryOrderCommandHandler(IEventSourcingHandler<DeliveryAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(AddOrderCommand command)
    {
        DeliveryAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.OrderId);
        aggregate.AddOrder(command.OrderId);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveOrderCommand command)
    {
        DeliveryAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.RemoveOrder(command.OrderId);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}