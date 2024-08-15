using CQRS.Core.Events;

namespace Post.Common.Events.Posts;

public class PostDeletedEvent : Event
{
    public PostDeletedEvent() : base(nameof(PostDeletedEvent))
    {
    }
}
