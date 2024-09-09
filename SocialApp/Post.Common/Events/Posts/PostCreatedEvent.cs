using CQRS.Core.Events;

namespace Post.Common.Events.Posts;

public class PostCreatedEvent : Event
{
    public PostCreatedEvent() : base(nameof(PostCreatedEvent))
    {
    }

    public string Author { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public DateTime DatePosted { get; set; }
}
