using CQRS.Core.Domain;
using Post.Command.Domain.Bos;
using Post.Command.Domain.Rules;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Posts;

namespace Post.Command.Domain;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _author;
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
            Id = id,
            Author = author,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }

    public void Apply(PostUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void Apply(PostDeletedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void Apply(CommentCreatedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new CommentBo { Author = @event.Username, Message = @event.Comment });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new CommentBo { Author = @event.Username, Message = @event.Comment };
    }

    public void Apply(CommentDeletedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void EditMessage(string message)
    {
        _active.CheckActiveRule("You cannot edit the message of an inactive post!");

        CheckMessageRule(message, $"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}!");

        RaiseEvent(new PostUpdatedEvent
        {
            Id = _id,
            Message = message
        });
    }

    private static void CheckMessageRule(string message, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public void LikePost()
    {
        _active.CheckActiveRule("You cannot like an inactive post!");

        RaiseEvent(new PostLikedEvent
        {
            Id = _id
        });
    }

    public void AddComment(string comment, string username)
    {
        _active.CheckActiveRule("You cannot add a comment to an inactive post!");

        comment.CheckCommentRule($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");

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
        _active.CheckActiveRule("You cannot edit a comment of an inactive post!");

        _comments[commentId].Author.CheckAuthorRule(username, "You are not allowed to edit a comment that was made by another user!");

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
        _active.CheckActiveRule("You cannot remove a comment of an inactive post!");

        _comments[commentId].Author.CheckAuthorRule(username, "You are not allowed to remove a comment that was made by another user!");

        RaiseEvent(new CommentDeletedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }

    public void DeletePost(string username)
    {
        _active.CheckActiveRule("The post has already been removed!");

        _author.CheckAuthorRule(username, "You are not allowed to remove a comment that was made by another user!");

        RaiseEvent(new PostDeletedEvent
        {
            Id = _id
        });
    }
}