using CQRS.Core;

namespace Post.Command.Api.Commands.Posts;

public class CreatePostCommand : BaseCommand
{
    public string? Author { get; set; }
    public string? Message { get; set; }
}
