using CQRS.Core.Events;

namespace Post.Common.Comments;

public class CommentCreatedEvent : Event
{
    public CommentCreatedEvent() : base(nameof(CommentCreatedEvent))
    {
    }

    public Guid CommentId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime CommentDate { get; set; }
}
