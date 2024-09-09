using CQRS.Core.Events;
using Post.Command.Api.Commands.Discounts;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Discounts;

public class DiscountCommandHandler : IDiscountCommandHandler
{
    private readonly IEventSourcingHandler<OrderAggregate> _eventSourcingHandler;

    public DiscountCommandHandler(IEventSourcingHandler<OrderAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreateDiscountCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.CreateDiscount(command.LowerThreshold, command.UpperThreshold, command.Percentage);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateDiscountCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.UpdateDiscount(command.DiscountId, command.LowerThreshold, command.UpperThreshold, command.Percentage);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeleteDiscountCommand command)
    {
        OrderAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.DeleteDiscount(command.DiscountId);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}