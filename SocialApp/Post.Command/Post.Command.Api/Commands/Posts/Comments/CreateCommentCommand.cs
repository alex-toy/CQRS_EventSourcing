using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Posts.Comments;

public class CreateCommentCommand : BaseCommand
{
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Guid PostId { get; set; }
}
