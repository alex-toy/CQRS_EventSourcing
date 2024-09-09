using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
using Post.Command.Domain.Rules;
using Post.Common.Events.Orders;
using Post.Common.Events.Orders.Discounts;
using Post.Common.Events.Orders.Items;

namespace Post.Command.Domain;

public class OrderAggregate : AggregateRoot
{
    private string _author;
    private readonly Dictionary<Guid, ItemBo> _items = new();
    private DiscountBo _discount;

    public OrderAggregate()
    {
    }

    public OrderAggregate(Guid id, string author, string address, bool isEmergency)
    {
        RaiseEvent(new OrderCreatedEvent
        {
            AggregateId = id,
            Author = author,
            Address = address,
            IsEmergency = isEmergency,
            CreatedAt = DateTime.UtcNow
        });
    }

    public void Apply(OrderCreatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _author = @event.Author;
    }

    public void Apply(OrderUpdatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void Apply(OrderDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void Apply(ItemCreatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _items.Add(@event.ItemId, new ItemBo { Label = @event.Label, Price = @event.Price, Quantity = @event.Quantity });
    }

    public void Apply(ItemUpdatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _items[@event.ItemId] = new ItemBo { Label = @event.Label, Price = @event.Price, Quantity = @event.Quantity };
    }

    public void Apply(ItemDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _items.Remove(@event.ItemId);
    }

    public void UpdateOrder(string address, bool isEmergency)
    {
        address.CheckAddressRule($"Please provide a valid {nameof(address)}!");

        RaiseEvent(new OrderUpdatedEvent
        {
            AggregateId = _aggregateId,
            Address = address,
            IsEmergency = isEmergency
        });
    }

    public void DeleteOrder(string author)
    {
        author.CheckAuthorRule("You are not allowed to delete an order that was made by someone else!");

        RaiseEvent(new OrderDeletedEvent
        {
            AggregateId = _aggregateId
        });
    }

    public void CreateItem(string label, double price, int quantity)
    {
        label.CheckLabelRules($"The value of {nameof(label)} cannot be null or empty. Please provide a valid {nameof(label)}!");

        RaiseEvent(new ItemCreatedEvent
        {
            AggregateId = _aggregateId,
            ItemId = Guid.NewGuid(),
            Label = label,
            Quantity = quantity,
            Price = price
        });
    }

    public void UpdateItem(Guid itemId, string label, double price, int quantity)
    {
        _items[itemId].Price.CheckPriceRule("Price should be positive");

        RaiseEvent(new ItemUpdatedEvent
        {
            AggregateId = _aggregateId,
            ItemId = itemId,
            Price = price,
            Label = label,
            Quantity = quantity,
            EditDate = DateTime.Now
        });
    }

    public void DeleteItem(Guid itemId)
    {
        RaiseEvent(new ItemDeletedEvent
        {
            AggregateId = _aggregateId,
            ItemId = itemId
        });
    }

    public void Apply(DiscountCreatedEvent @event)
    {
        _discount.CheckDiscountUnicityRule();
        @event.CheckDiscountRules();

        _aggregateId = @event.AggregateId;
        _discount = new DiscountBo
        {
            LowerThreshold = @event.LowerThreshold,
            UpperThreshold = @event.UpperThreshold,
            Percentage = @event.Percentage
        };
    }

    public void Apply(DiscountUpdatedEvent @event)
    {
        @event.CheckDiscountRules();

        _aggregateId = @event.AggregateId;
        _discount = new DiscountBo
        {
            LowerThreshold = @event.LowerThreshold,
            UpperThreshold = @event.UpperThreshold,
            Percentage = @event.Percentage
        };
    }

    public void Apply(DiscountDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _discount = null;
    }

    public void CreateDiscount(double lowerThreshold, double upperThreshold, double percentage)
    {
        RaiseEvent(new DiscountCreatedEvent
        {
            AggregateId = _aggregateId,
            DiscountId = Guid.NewGuid(),
            LowerThreshold = lowerThreshold,
            UpperThreshold = upperThreshold,
            Percentage = percentage
        });
    }

    public void UpdateDiscount(Guid discountId, double lowerThreshold, double upperThreshold, double percentage)
    {
        RaiseEvent(new DiscountUpdatedEvent
        {
            AggregateId = _aggregateId,
            DiscountId = discountId,
            LowerThreshold = lowerThreshold,
            UpperThreshold = upperThreshold,
            Percentage = percentage,
            EditDate = DateTime.Now
        });
    }

    public void DeleteDiscount(Guid discountId)
    {
        RaiseEvent(new DiscountDeletedEvent
        {
            AggregateId = _aggregateId,
            DiscountId = discountId
        });
    }
}