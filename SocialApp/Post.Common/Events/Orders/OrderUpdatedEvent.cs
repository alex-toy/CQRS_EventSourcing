using CQRS.Core.Events;

namespace Post.Common.Events.Orders;

public class OrderUpdatedEvent : Event
{
    public OrderUpdatedEvent() : base(nameof(OrderUpdatedEvent))
    {
    }

    public string Address { get; set; }
    public bool IsEmergency { get; set; }
}
