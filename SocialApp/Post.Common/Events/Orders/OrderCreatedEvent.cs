using CQRS.Core.Events;

namespace Post.Common.Events.Orders;

public class OrderCreatedEvent : Event
{
    public OrderCreatedEvent() : base(nameof(OrderCreatedEvent))
    {
    }

    public string Author { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}
