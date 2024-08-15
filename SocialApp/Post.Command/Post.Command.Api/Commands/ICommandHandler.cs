using Post.Command.Api.Commands.Comments;
using Post.Command.Api.Commands.Posts;

namespace Post.Command.Api.Commands;

public interface ICommandHandler
{
    Task HandleAsync(CreatePostCommand command);
    Task HandleAsync(UpdatePostCommand command);
    Task HandleAsync(LikePostCommand command);
    Task HandleAsync(CreateCommentCommand command);
    Task HandleAsync(UpdateCommentCommand command);
    Task HandleAsync(DeleteCommentCommand command);
    Task HandleAsync(DeletePostCommand command);
    Task HandleAsync(RestoreReadDbCommand command);
}