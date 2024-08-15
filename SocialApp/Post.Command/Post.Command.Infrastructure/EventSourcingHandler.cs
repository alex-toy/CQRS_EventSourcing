using CQRS.Core.Domain;
using CQRS.Core.Events;
using Post.Command.Domain;

namespace Post.Command.Infrastructure;

public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
    {
        _eventStore = eventStore;
        _eventProducer = eventProducer;
    }

    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        PostAggregate aggregate = new ();
        List<Event> events = await _eventStore.GetEventsAsync(aggregateId);

        if (events is null || !events.Any()) return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();

        return aggregate;
    }

    public async Task RepublishEventsAsync()
    {
        List<Guid> aggregateIds = await _eventStore.GetAggregateIdsAsync();

        if (aggregateIds is null || !aggregateIds.Any()) return;

        foreach (var aggregateId in aggregateIds)
        {
            PostAggregate aggregate = await GetByIdAsync(aggregateId);

            if (aggregate is null || !aggregate.Active) continue;

            List<Event> events = await _eventStore.GetEventsAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}