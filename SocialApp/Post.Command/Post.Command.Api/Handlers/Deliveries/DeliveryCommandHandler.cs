using CQRS.Core.Events;
using Post.Command.Api.Commands.Deliveries;
using Post.Command.Api.Commands.Deliveries.Orders;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Deliveries;

public class DeliveryCommandHandler : IDeliveryCommandHandler
{
    private readonly IEventSourcingHandler<DeliveryAggregate> _eventSourcingHandler;

    public DeliveryCommandHandler(IEventSourcingHandler<DeliveryAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreateDeliveryCommand command)
    {
        DeliveryAggregate aggregate = new(command.AggregateId, command.DriverName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateDeliveryCommand command)
    {
        DeliveryAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.EditDriverName(command.DriverName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeleteDeliveryCommand command)
    {
        DeliveryAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.Delete();

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}