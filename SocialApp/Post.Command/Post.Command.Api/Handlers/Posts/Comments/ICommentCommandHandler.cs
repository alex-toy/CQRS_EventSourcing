using Post.Command.Api.Commands.Posts.Comments;

namespace Post.Command.Api.Handlers.Posts.Comments;

public interface ICommentCommandHandler
{
    Task HandleAsync(CreateCommentCommand command);
    Task HandleAsync(UpdateCommentCommand command);
    Task HandleAsync(DeleteCommentCommand command);
}