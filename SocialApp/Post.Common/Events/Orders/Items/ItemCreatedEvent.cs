using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Items;

public class ItemCreatedEvent : Event
{
    public ItemCreatedEvent() : base(nameof(ItemCreatedEvent))
    {
    }

    public Guid ItemId { get; set; }
    public string Label { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
}
