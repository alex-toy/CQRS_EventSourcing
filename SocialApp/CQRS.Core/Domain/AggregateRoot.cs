using CQRS.Core.Events;
using System.Reflection;

namespace CQRS.Core.Domain;

public abstract class AggregateRoot
{
    protected Guid _aggregateId;
    private readonly List<Event> _changes = new();
    public virtual bool Active { get; set; }

    public Guid Id
    {
        get { return _aggregateId; }
    }

    public int Version { get; set; } = -1;

    public void ReplayEvents(IEnumerable<Event> events)
    {
        foreach (var @event in events) ApplyChange(@event, false);
    }

    public IEnumerable<Event> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    private void ApplyChange(Event @event, bool isNew)
    {
        MethodInfo? method = GetType().GetMethod("Apply", new Type[] { @event.GetType() });

        if (method is null)
        {
            throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}!");
        }

        method.Invoke(this, new object[] { @event });

        if (isNew) _changes.Add(@event);
    }

    protected void RaiseEvent(Event @event)
    {
        ApplyChange(@event, true);
    }
}