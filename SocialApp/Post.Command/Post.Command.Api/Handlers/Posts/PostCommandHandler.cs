using CQRS.Core.Events;
using Post.Command.Api.Commands;
using Post.Command.Api.Commands.Posts;
using Post.Command.Domain;

namespace Post.Command.Api.Handlers.Posts;

public class PostCommandHandler : IPostCommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    public PostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(CreatePostCommand command)
    {
        PostAggregate aggregate = new(command.AggregateId, command.Author, command.Message);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdatePostCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.EditMessage(command.Message);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.LikePost();

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        PostAggregate aggregate = await _eventSourcingHandler.GetByIdAsync(command.AggregateId);
        aggregate.DeletePost(command.UserName);

        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RestoreReadDbCommand command)
    {
        await _eventSourcingHandler.RepublishEventsAsync();
    }
}