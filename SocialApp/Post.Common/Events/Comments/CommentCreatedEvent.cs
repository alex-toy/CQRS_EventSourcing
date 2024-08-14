using CQRS.Core;

namespace Post.Common.Comments;

public class CommentCreatedEvent : Event
{
    public CommentCreatedEvent() : base(nameof(CommentCreatedEvent))
    {
    }

    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string Username { get; set; }
    public DateTime CommentDate { get; set; }
}
