using CQRS.Core;

namespace Post.Common.Events.Posts;

public class PostLikedEvent : Event
{
    public PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}
