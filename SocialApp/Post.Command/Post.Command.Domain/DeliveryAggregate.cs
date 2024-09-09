using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
using Post.Command.Domain.Rules;
using Post.Common.Events.Deliveries;

namespace Post.Command.Domain;

public class DeliveryAggregate : AggregateRoot
{
    private string _driverName = string.Empty;
    private readonly Dictionary<Guid, CommentBo> _orders = new();

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

    public void EditDriverName(string driverName)
    {
        driverName.CheckDriverNameRule($"The value of {nameof(driverName)} cannot be null or empty. Please provide a valid {nameof(driverName)}!");

        RaiseEvent(new DeliveryUpdatedEvent
        {
            AggregateId = _aggregateId,
            DriverName = driverName
        });
    }
}
