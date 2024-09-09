using CQRS.Core.Events;

namespace Post.Common.Events.Deliveries;

public class DeliveryCreatedEvent : Event
{
    public DeliveryCreatedEvent() : base(nameof(DeliveryCreatedEvent))
    {
    }

    public string DriverName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
