using CQRS.Core.Events;

namespace Post.Common.Events.Deliveries.Orders;

public class OrderAddedEvent : Event
{
    public OrderAddedEvent() : base(nameof(OrderAddedEvent))
    {
    }

    public Guid OrderId { get; set; }
}
