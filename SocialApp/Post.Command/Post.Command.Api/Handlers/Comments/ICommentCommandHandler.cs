using Post.Command.Api.Commands;
using Post.Command.Api.Commands.Comments;
using Post.Command.Api.Commands.Posts;

namespace Post.Command.Api.Handlers.Comments;

public interface ICommentCommandHandler
{
    Task HandleAsync(CreateCommentCommand command);
    Task HandleAsync(UpdateCommentCommand command);
    Task HandleAsync(DeleteCommentCommand command);
}