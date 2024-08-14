using CQRS.Core;

namespace Post.Command.Api.Commands.Comments;

public class DeleteCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
}
