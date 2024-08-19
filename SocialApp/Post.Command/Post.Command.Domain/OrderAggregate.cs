using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
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
            Id = id,
            Author = author,
            Address = address,
            IsEmergency = isEmergency
        });
    }

    public void Apply(OrderCreatedEvent @event)
    {
        _id = @event.Id;
        _author = @event.Author;
    }

    public void Apply(OrderUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void Apply(OrderDeletedEvent @event)
    {
        _id = @event.Id;
    }

    public void Apply(ItemCreatedEvent @event)
    {
        _id = @event.Id;
        _items.Add(@event.ItemId, new ItemBo { Label = @event.Label, Price = @event.Price, Quantity = @event.Quantity });
    }

    public void Apply(ItemUpdatedEvent @event)
    {
        _id = @event.Id;
        _items[@event.ItemId] = new ItemBo { Label = @event.Label, Price = @event.Price, Quantity = @event.Quantity };
    }

    public void Apply(ItemDeletedEvent @event)
    {
        _id = @event.Id;
        _items.Remove(@event.ItemId);
    }

    public void UpdateOrder(string address, bool isEmergency)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new InvalidOperationException($"Please provide a valid {nameof(address)}!");
        }

        RaiseEvent(new OrderUpdatedEvent
        {
            Id = _id,
            Address = address,
            IsEmergency = isEmergency
        });
    }

    public void DeleteOrder(string author)
    {
        if (!_author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to delete an order that was made by someone else!");
        }

        RaiseEvent(new OrderDeletedEvent
        {
            Id = _id
        });
    }

    public void CreateItem(string label, double price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new InvalidOperationException($"The value of {nameof(label)} cannot be null or empty. Please provide a valid {nameof(label)}!");
        }

        RaiseEvent(new ItemCreatedEvent
        {
            Id = _id,
            ItemId = Guid.NewGuid(),
            Label = label,
            Quantity = quantity,
            Price = price
        });
    }

    public void UpdateItem(Guid itemId, string label, double price, int quantity)
    {
        //if (!_items[itemId].Item2.Equals(price, StringComparison.CurrentCultureIgnoreCase))
        //{
        //    throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
        //}

        RaiseEvent(new ItemUpdatedEvent
        {
            Id = _id,
            ItemId = itemId,
            Price = price,
            Label = label,
            Quantity = quantity,
            EditDate = DateTime.Now
        });
    }

    public void DeleteItem(Guid itemId)
    {
        //if (!_items[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        //{
        //    throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user!");
        //}

        RaiseEvent(new ItemDeletedEvent
        {
            Id = _id,
            ItemId = itemId
        });
    }

    public void Apply(DiscountCreatedEvent @event)
    {
        if (_discount is not null) throw new InvalidOperationException("You already have a discount!");
        _id = @event.Id;
        _discount = new DiscountBo { 
            LowerThreshold = @event.LowerThreshold,
            UpperThreshold = @event.UpperThreshold,
            Percentage = @event.Percentage
        };
    }

    public void Apply(DiscountUpdatedEvent @event)
    {
        _id = @event.Id;
        _discount = new DiscountBo
        {
            LowerThreshold = @event.LowerThreshold,
            UpperThreshold = @event.UpperThreshold,
            Percentage = @event.Percentage
        };
    }

    public void Apply(DiscountDeletedEvent @event)
    {
        _id = @event.Id;
        _discount = null;
    }

    public void CreateDiscount(double lowerThreshold, double upperThreshold, double percentage)
    {
        RaiseEvent(new DiscountCreatedEvent
        {
            Id = _id,
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
            Id = _id,
            DiscountId = Guid.NewGuid(),
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
            Id = _id,
            DiscountId = discountId
        });
    }
}