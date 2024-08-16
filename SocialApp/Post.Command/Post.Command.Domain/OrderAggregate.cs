using CQRS.Core.Domain;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Orders;

namespace Post.Command.Domain;

public class OrderAggregate : AggregateRoot
{
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public OrderAggregate()
    {
    }

    public OrderAggregate(Guid id, Guid itemId, int quantity)
    {
        RaiseEvent(new OrderCreatedEvent
        {
            Id = id,
            Quantity = quantity,
            ItemId = itemId
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
        _author = @event.Author;
    }

    //public void Apply(CommentCreatedEvent @event)
    //{
    //    _id = @event.Id;
    //    _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    //}

    //public void Apply(CommentUpdatedEvent @event)
    //{
    //    _id = @event.Id;
    //    _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
    //}

    public void Apply(OrderDeletedEvent @event)
    {
        _id = @event.Id;
    }

    //public void Apply(CommentDeletedEvent @event)
    //{
    //    _id = @event.Id;
    //    _comments.Remove(@event.CommentId);
    //}

    public void EditQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new InvalidOperationException($"The value of {nameof(quantity)} cannot be negative. Please provide a valid {nameof(quantity)}!");
        }

        RaiseEvent(new OrderUpdatedEvent
        {
            Id = _id,
            Quantity = quantity
        });
    }

    public void DeleteOrder(string author)
    {
        if (!_author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to delete a post that was made by someone else!");
        }

        RaiseEvent(new OrderDeletedEvent
        {
            Id = _id
        });
    }

    public void AddComment(string comment, string username)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");
        }

        RaiseEvent(new CommentCreatedEvent
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.Now
        });
    }

    public void EditComment(Guid commentId, string comment, string username)
    {
        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
        }

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            Username = username,
            EditDate = DateTime.Now
        });
    }

    public void RemoveComment(Guid commentId, string username)
    {
        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user!");
        }

        RaiseEvent(new CommentDeletedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }
}