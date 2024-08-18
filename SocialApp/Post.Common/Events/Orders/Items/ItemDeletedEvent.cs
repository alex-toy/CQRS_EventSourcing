using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Items;

public class ItemDeletedEvent : Event
{
    public ItemDeletedEvent() : base(nameof(ItemDeletedEvent))
    {
    }

    public Guid ItemId { get; set; }
}
