using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Discounts;

public class DiscountDeletedEvent : Event
{
    public DiscountDeletedEvent() : base(nameof(DiscountDeletedEvent))
    {
    }

    public Guid DiscountId { get; set; }
}
