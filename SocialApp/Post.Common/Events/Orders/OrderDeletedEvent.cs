using CQRS.Core.Events;

namespace Post.Common.Events.Orders;

public class OrderDeletedEvent : Event
{
    public OrderDeletedEvent() : base(nameof(OrderDeletedEvent))
    {
    }
}
