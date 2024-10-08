﻿using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
using Post.Command.Domain.Rules;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Deliveries.Orders;
using Post.Common.Events.Posts;

namespace Post.Command.Domain;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _author = string.Empty;
    private readonly Dictionary<Guid, CommentBo> _comments = new();

    public override bool Active
    {
        get => _active; set => _active = value;
    }

    public PostAggregate()
    {
    }

    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostCreatedEvent
        {
            AggregateId = id,
            Author = author,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _active = true;
        _author = @event.Author;
    }

    public void Apply(PostUpdatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void Apply(PostDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _active = false;
    }

    public void Apply(PostLikedEvent @event)
    {
        _aggregateId = @event.AggregateId;
    }

    public void Apply(CommentCreatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _comments.Add(@event.CommentId, new CommentBo { Author = @event.Username, Message = @event.Comment });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _comments[@event.CommentId] = new CommentBo { Author = @event.Username, Message = @event.Comment };
    }

    public void Apply(CommentDeletedEvent @event)
    {
        _aggregateId = @event.AggregateId;
        _comments.Remove(@event.CommentId);
    }

    public void EditMessage(string message)
    {
        _active.CheckActiveRule("You cannot edit the message of an inactive post!");

        message.CheckMessageRule($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}!");

        RaiseEvent(new PostUpdatedEvent
        {
            AggregateId = _aggregateId,
            Message = message
        });
    }

    public void LikePost()
    {
        _active.CheckActiveRule("You cannot like an inactive post!");

        RaiseEvent(new PostLikedEvent
        {
            AggregateId = _aggregateId
        });
    }

    public void AddComment(string comment, string username)
    {
        _active.CheckActiveRule("You cannot add a comment to an inactive post!");

        comment.CheckCommentRule($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");

        RaiseEvent(new CommentCreatedEvent
        {
            AggregateId = _aggregateId,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.Now
        });
    }

    public void EditComment(Guid commentId, string comment, string username)
    {
        _active.CheckActiveRule("You cannot edit a comment of an inactive post!");

        _comments[commentId].Author.CheckAuthorRule(username, "You are not allowed to edit a comment that was made by another user!");

        RaiseEvent(new CommentUpdatedEvent
        {
            AggregateId = _aggregateId,
            CommentId = commentId,
            Comment = comment,
            Username = username,
            EditDate = DateTime.Now
        });
    }

    public void RemoveComment(Guid commentId, string username)
    {
        _active.CheckActiveRule("You cannot remove a comment of an inactive post!");

        _comments[commentId].Author.CheckAuthorRule(username, "You are not allowed to remove a comment that was made by another user!");

        RaiseEvent(new CommentDeletedEvent
        {
            AggregateId = _aggregateId,
            CommentId = commentId
        });
    }

    public void DeletePost(string username)
    {
        _active.CheckActiveRule("The post has already been removed!");

        _author.CheckAuthorRule(username, "You are not allowed to remove a comment that was made by another user!");

        RaiseEvent(new PostDeletedEvent
        {
            AggregateId = _aggregateId
        });
    }
}