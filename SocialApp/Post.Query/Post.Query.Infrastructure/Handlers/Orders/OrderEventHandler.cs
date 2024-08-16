using Post.Common.Events.Orders;
using Post.Query.Domain.Entities;
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
            CreatedAt = @event.CreatedAt
        };

        await _orderRepository.CreateAsync(order);
    }

    //public async Task On(OrderUpdatedEvent @event)
    //{
    //    OrderDb post = await _orderRepository.GetByIdAsync(@event.Id);

    //    if (post is null) return;

    //    post.Message = @event.Message;
    //    await _orderRepository.UpdateAsync(post);
    //}

    public async Task On(OrderDeletedEvent @event)
    {
        await _orderRepository.DeleteAsync(@event.Id);
    }
}