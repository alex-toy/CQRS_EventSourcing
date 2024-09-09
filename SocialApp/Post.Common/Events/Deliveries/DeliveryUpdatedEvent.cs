using CQRS.Core.Events;

namespace Post.Common.Events.Deliveries;

public class DeliveryUpdatedEvent : Event
{
    public DeliveryUpdatedEvent() : base(nameof(DeliveryUpdatedEvent))
    {
    }

    public string DriverName { get; set; }
}
