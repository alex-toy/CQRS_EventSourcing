using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Posts;

public class DeletePostCommand : BaseCommand
{
    public Guid PostId { get; set; }
    public string UserName { get; set; }
}
