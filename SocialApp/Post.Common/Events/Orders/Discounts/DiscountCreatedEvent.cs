using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Discounts;

public class DiscountCreatedEvent : Event
{
    public DiscountCreatedEvent() : base(nameof(DiscountCreatedEvent))
    {
    }

    public Guid DiscountId { get; set; }
    public double LowerThreshold { get; set; }
    public double UpperThreshold { get; set; }
    public double Percentage { get; set; }
}
