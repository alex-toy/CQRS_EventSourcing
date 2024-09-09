using CQRS.Core.Events;
using Post.Command.Api.Commands.Deliveries;
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

    //public async Task HandleAsync(LikePostCommand command)
    //{
    //    PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
    //    aggregate.LikePost();

    //    await _eventSourcingHandler.SaveAsync(aggregate);
    //}

    //public async Task HandleAsync(DeletePostCommand command)
    //{
    //    PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
    //    aggregate.DeletePost(command.UserName);

    //    await _eventSourcingHandler.SaveAsync(aggregate);
    //}

    //public async Task HandleAsync(RestoreReadDbCommand command)
    //{
    //    await _eventSourcingHandler.RepublishEventsAsync();
    //}
}