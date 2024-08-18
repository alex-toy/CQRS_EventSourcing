using Post.Common.Events.Orders;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers.Orders;

public class OrderEventHandler : IOrderEventHandler
{
    private readonly IOrderRepository _orderRepository;

    public OrderEventHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task On(OrderCreatedEvent @event)
    {
        var order = new OrderDb
        {
            OrderId = @event.Id,
            Author = @event.Author,
            Address = @event.Address,
            IsEmergency = @event.IsEmergency,
            CreatedAt = @event.CreatedAt
        };

        await _orderRepository.CreateAsync(order);
    }

    public async Task On(OrderUpdatedEvent @event)
    {
        OrderDb order = await _orderRepository.GetByIdAsync(@event.Id);

        if (order is null) return;

        order.Address = @event.Address;
        order.IsEmergency = @event.IsEmergency;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task On(OrderDeletedEvent @event)
    {
        await _orderRepository.DeleteAsync(@event.Id);
    }
}