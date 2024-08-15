using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Posts;

public class UpdatePostCommand : BaseCommand
{
    public string Message { get; set; }
}
