using CQRS.Core;

namespace Post.Command.Api.Commands.Posts;

public class UpdatePostCommand : BaseCommand
{
    public string Message { get; set; }
}
