namespace CQRS.Core.Events;

public abstract class Event
{
    protected Event(string type)
    {
        Type = type;
    }

    public int Version { get; set; }
    public string Type { get; set; }
    public Guid AggregateId { get; set; }
}
