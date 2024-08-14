using CQRS.Core;

namespace Post.Common.Events.Posts;

public class PostCreatedEvent : Event
{
    public PostCreatedEvent() : base(nameof(PostCreatedEvent))
    {
    }

    public string Author { get; set; }
    public string Message { get; set; }
    public DateTime DatePosted { get; set; }
}
