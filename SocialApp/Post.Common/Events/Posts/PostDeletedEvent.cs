using CQRS.Core;

namespace Post.Common.Events.Posts;

public class PostDeletedEvent : Event
{
    public PostDeletedEvent() : base(nameof(PostDeletedEvent))
    {
    }
}
