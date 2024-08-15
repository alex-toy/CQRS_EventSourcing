using CQRS.Core.Events;

namespace Post.Common.Events.Comments;

public class CommentDeletedEvent : Event
{
    public CommentDeletedEvent() : base(nameof(CommentDeletedEvent))
    {
    }

    public Guid CommentId { get; set; }
}
