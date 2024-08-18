using CQRS.Core.Domain;

namespace CQRS.Core.Events;

public interface IEventStore
{
    //Task SaveEventsAsync(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
    Task SaveEventsAsync<TAggregate>(Guid aggregateId, IEnumerable<Event> events, int expectedVersion) where TAggregate : AggregateRoot, new();
    Task<List<Event>> GetEventsAsync(Guid aggregateId);
    Task<List<Guid>> GetAggregateIdsAsync();
}
