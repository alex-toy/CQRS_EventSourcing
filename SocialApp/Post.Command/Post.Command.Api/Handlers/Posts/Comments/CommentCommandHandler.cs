using CQRS.Core.Events;
using Post.Command.Api.Commands.Posts.Comments;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Posts.Comments;

public class CommentCommandHandler : ICommentCommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    public CommentCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreateCommentCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.AddComment(command.Comment, command.UserName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateCommentCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.UserName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeleteCommentCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.UserName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}