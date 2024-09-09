using CQRS.Core.Events;

namespace Post.Common.Events.Deliveries.Orders;

public class OrderRemovedEvent : Event
{
    public OrderRemovedEvent() : base(nameof(OrderRemovedEvent))
    {
    }

    public Guid OrderId { get; set; }
}
