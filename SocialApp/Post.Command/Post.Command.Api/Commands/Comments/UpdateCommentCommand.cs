using CQRS.Core;

namespace Post.Command.Api.Commands.Comments;

public class UpdateCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string UserName { get; set; }
}
