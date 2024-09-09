namespace CQRS.Core.Commands;

public abstract class BaseCommand
{
    public Guid AggregateId { get; set; }
}
