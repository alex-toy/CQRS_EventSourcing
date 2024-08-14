using CQRS.Core;

namespace Post.Common.Events.Posts;

public class PostUpdatedEvent : Event
{
    public PostUpdatedEvent() : base(nameof(PostUpdatedEvent))
    {
    }

    public string Message { get; set; }
}
