using CQRS.Core.Events;

namespace Post.Common.Events.Orders;

public class OrderCreatedEvent : Event
{
    public OrderCreatedEvent() : base(nameof(OrderCreatedEvent))
    {
    }

    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Address { get; set; }
    public bool IsEmergency { get; set; }
}
