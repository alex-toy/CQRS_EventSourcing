namespace CQRS.Core.Events;

public interface IEventConsumer
{
    void Consume(string topic);
}