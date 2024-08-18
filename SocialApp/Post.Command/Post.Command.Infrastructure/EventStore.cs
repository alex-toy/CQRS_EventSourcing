using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;

namespace Post.Command.Infrastructure;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }

    public async Task<List<Guid>> GetAggregateIdsAsync()
    {
        List<EventModel> eventStream = await _eventStoreRepository.FindAllAsync();

        if (eventStream is null || !eventStream.Any())
        {
            throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");
        }

        return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
    }

    public async Task<List<Event>> GetEventsAsync(Guid aggregateId)
    {
        List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId); //label = null !!!

        if (eventStream is null || !eventStream.Any())
        {
            throw new AggregateNotFoundException("Incorrect post ID provided!");
        }

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }

    public async Task SaveEventsAsync<TAggregate>(Guid aggregateId, IEnumerable<Event> events, int expectedVersion) where TAggregate : AggregateRoot, new()
    {
        List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException();
        }

        int version = expectedVersion;

        foreach (Event @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            EventModel eventModel = new ()
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(TAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _eventStoreRepository.SaveAsync(eventModel);

            string? topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
}