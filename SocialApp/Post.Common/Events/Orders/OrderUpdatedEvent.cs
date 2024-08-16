using CQRS.Core.Events;

namespace Post.Common.Events.Orders;

public class OrderUpdatedEvent : Event
{
    public OrderUpdatedEvent() : base(nameof(OrderUpdatedEvent))
    {
    }

    public string Author { get; set; }
    public int Quantity { get; set; }
}
