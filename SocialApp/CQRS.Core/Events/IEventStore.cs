namespace CQRS.Core.Events;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
    Task<List<Event>> GetEventsAsync(Guid aggregateId);
    Task<List<Guid>> GetAggregateIdsAsync();
}
