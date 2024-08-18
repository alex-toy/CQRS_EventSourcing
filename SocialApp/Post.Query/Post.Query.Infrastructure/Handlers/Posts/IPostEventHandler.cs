using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Posts;

namespace Post.Query.Infrastructure.Handlers.Posts;

public interface IPostEventHandler : IEventHandler
{
    Task On(PostCreatedEvent @event);
    Task On(PostUpdatedEvent @event);
    Task On(PostLikedEvent @event);
    Task On(PostDeletedEvent @event);
    Task On(CommentCreatedEvent @event);
    Task On(CommentUpdatedEvent @event);
    Task On(CommentDeletedEvent @event);
}