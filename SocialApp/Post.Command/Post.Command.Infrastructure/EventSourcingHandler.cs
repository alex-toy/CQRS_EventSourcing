using CQRS.Core.Domain;
using CQRS.Core.Events;

namespace Post.Command.Infrastructure;

public class EventSourcingHandler<T> : IEventSourcingHandler<T> where T : AggregateRoot, new()
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
    {
        _eventStore = eventStore;
        _eventProducer = eventProducer;
    }

    public async Task<T> GetByIdAsync(Guid aggregateId)
    {
        T aggregate = new ();
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
            T aggregate = await GetByIdAsync(aggregateId);

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
        await _eventStore.SaveEventsAsync<T>(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}