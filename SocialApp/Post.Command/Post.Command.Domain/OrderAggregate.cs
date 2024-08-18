using Amazon.Runtime.Internal.Transform;
using CQRS.Core.Domain;
using Post.Common.Events.Orders;
using Post.Common.Events.Orders.Items;

namespace Post.Command.Domain;

public class OrderAggregate : AggregateRoot
{
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, double, int>> _items = new();

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
        _items.Add(@event.ItemId, new Tuple<string, double, int>(@event.Label, @event.Price, @event.Quantity));
    }

    public void Apply(ItemUpdatedEvent @event)
    {
        _id = @event.Id;
        _items[@event.ItemId] = new Tuple<string, double, int>(@event.Label, @event.Price, @event.Quantity);
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

    public void AddItem(string label, double price, int quantity)
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

    public void EditItem(Guid itemId, string label, double price, int quantity)
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

    public void RemoveItem(Guid itemId)
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
}