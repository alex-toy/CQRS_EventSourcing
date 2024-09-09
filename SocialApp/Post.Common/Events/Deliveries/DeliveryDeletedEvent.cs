using CQRS.Core.Events;

namespace Post.Common.Events.Deliveries;

public class DeliveryDeletedEvent : Event
{
    public DeliveryDeletedEvent() : base(nameof(DeliveryDeletedEvent))
    {
    }
}
