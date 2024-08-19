using CQRS.Core.Events;

namespace Post.Common.Events.Orders.Discounts;

public class DiscountUpdatedEvent : Event
{
    public DiscountUpdatedEvent() : base(nameof(DiscountUpdatedEvent))
    {
    }

    public Guid DiscountId { get; set; }
    public double LowerThreshold { get; set; }
    public double UpperThreshold { get; set; }
    public double Percentage { get; set; }
    public DateTime EditDate { get; set; }
}
