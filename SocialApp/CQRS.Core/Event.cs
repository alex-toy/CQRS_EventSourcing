namespace CQRS.Core;

public abstract class Event : Message
{
    protected Event(string type)
    {
        Type = type;
    }

    public int Version { get; set; }
    public string Type { get; set; }
}
