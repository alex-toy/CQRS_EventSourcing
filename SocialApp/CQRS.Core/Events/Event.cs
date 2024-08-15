namespace CQRS.Core.Events;

public abstract class Event : Message
{
    protected Event(string type)
    {
        Type = type;
    }

    public int Version { get; set; }
    public string Type { get; set; }
}
