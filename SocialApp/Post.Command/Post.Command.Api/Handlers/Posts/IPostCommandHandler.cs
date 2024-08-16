using Post.Command.Api.Commands;
using Post.Command.Api.Commands.Posts;

namespace Post.Command.Api.Handlers.Posts;

public interface IPostCommandHandler
{
    Task HandleAsync(CreatePostCommand command);
    Task HandleAsync(UpdatePostCommand command);
    Task HandleAsync(LikePostCommand command);
    Task HandleAsync(DeletePostCommand command);
    Task HandleAsync(RestoreReadDbCommand command);
}