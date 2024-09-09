using Post.Common.Events.Orders;
using Post.Common.Events.Orders.Items;

namespace Post.Query.Infrastructure.Handlers.Orders;

public interface IOrderEventHandler : IEventHandler
{
    Task On(OrderCreatedEvent @event);
    Task On(OrderUpdatedEvent @event);
    Task On(OrderDeletedEvent @event);
    Task On(ItemCreatedEvent @event);
    Task On(ItemUpdatedEvent @event);
    Task On(ItemDeletedEvent @event);
}