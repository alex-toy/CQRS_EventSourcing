using Post.Common.Events.Orders;

namespace Post.Query.Infrastructure.Handlers.Orders;

public interface IOrderEventHandler : IEventHandler
{
    Task On(OrderCreatedEvent @event);
    //Task On(OrderUpdatedEvent @event);
    Task On(OrderDeletedEvent @event);
}