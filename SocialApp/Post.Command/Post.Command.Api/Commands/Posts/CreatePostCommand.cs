using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Posts;

public class CreatePostCommand : BaseCommand
{
    public string Author { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
