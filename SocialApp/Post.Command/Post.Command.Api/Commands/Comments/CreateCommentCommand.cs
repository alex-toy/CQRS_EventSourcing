using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Comments;

public class CreateCommentCommand : BaseCommand
{
    public string Comment { get; set; }
    public string UserName { get; set; }
}
