using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
using Post.Command.Domain.Rules;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Deliveries;
using Post.Common.Events.Deliveries.Orders;

namespace Post.Command.Domain;

public class DeliveryAggregate : AggregateRoot
{
    private string _driverName = string.Empty;
    private readonly List<Guid> _orders = new();

    public DeliveryAggregate()
    {
    }

    public DeliveryAggregate(Guid id, string driverName)
    {
        RaiseEvent(new DeliveryCreatedEvent
        {
            AggregateId = id,
            DriverName = driverName,
            CreatedAt = DateTime.UtcNow
        });
    }

    public void Apply(DeliveryCreatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _driverName = @event.DriverName;
    }

    public void Apply(DeliveryUpdatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void Apply(DeliveryDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void EditDriverName(string driverName)
    {
        driverName.CheckDriverNameRule($"The value of {nameof(driverName)} cannot be null or empty. Please provide a valid {nameof(driverName)}!");

        RaiseEvent(new DeliveryUpdatedEvent
        {
            AggregateId = _aggregateId,
            DriverName = driverName
        });
    }

    public void Delete()
    {
        RaiseEvent(new DeliveryDeletedEvent
        {
            AggregateId = _aggregateId
        });
    }

    public void Apply(OrderAddedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _orders.Add(@event.OrderId);
    }

    public void Apply(OrderRemovedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _orders.Remove(@event.OrderId);
    }

    public void AddOrder(Guid orderId)
    {
        RaiseEvent(new OrderAddedEvent
        {
            AggregateId = _aggregateId,
            OrderId = orderId
        });
    }

    public void RemoveOrder(Guid orderId)
    {
        RaiseEvent(new OrderRemovedEvent
        {
            AggregateId = _aggregateId,
            OrderId = orderId
        });
    }
}
