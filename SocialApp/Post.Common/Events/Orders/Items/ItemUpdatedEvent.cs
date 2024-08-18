using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Items;

public class ItemUpdatedEvent : Event
{
    public ItemUpdatedEvent() : base(nameof(ItemUpdatedEvent))
    {
    }

    public Guid ItemId { get; set; }
    public string Label { get; set; }
    public double Price { get; set; }
    public DateTime EditDate { get; set; }
}
