using Post.Common.Events.Orders;
using Post.Common.Events.Orders.Discounts;
using Post.Common.Events.Orders.Items;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;

namespace Post.Query.Infrastructure.Handlers.Orders;

public class OrderEventHandler : IOrderEventHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IDiscountRepository _discountRepository;

    public OrderEventHandler(IOrderRepository orderRepository, IItemRepository itemRepository, IDiscountRepository discountRepository)
    {
        _orderRepository = orderRepository;
        _itemRepository = itemRepository;
        _discountRepository = discountRepository;
    }

    public async Task On(OrderCreatedEvent @event)
    {
        var order = new OrderDb
        {
            OrderId = @event.AggregateId,
            Author = @event.Author,
            Address = @event.Address,
            IsEmergency = @event.IsEmergency,
            CreatedAt = @event.CreatedAt
        };

        await _orderRepository.CreateAsync(order);
    }

    public async Task On(OrderUpdatedEvent @event)
    {
        OrderDb? order = await _orderRepository.GetByIdAsync(@event.AggregateId);

        if (order is null) return;

        order.Address = @event.Address;
        order.IsEmergency = @event.IsEmergency;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task On(OrderDeletedEvent @event)
    {
        await _orderRepository.DeleteAsync(@event.AggregateId);
    }

    public async Task On(ItemCreatedEvent @event)
    {
        var comment = new ItemDb
        {
            OrderId = @event.AggregateId,
            ItemId = @event.ItemId,
            Label = @event.Label,
            Price = @event.Price,
            Quantity = @event.Quantity
        };

        await _itemRepository.CreateAsync(comment);
    }

    public async Task On(ItemUpdatedEvent @event)
    {
        ItemDb? item = await _itemRepository.GetByIdAsync(@event.ItemId);

        if (item is null) return;

        item.Label = @event.Label;
        item.Price = @event.Price;
        item.Quantity = @event.Quantity;

        await _itemRepository.UpdateAsync(item);
    }

    public async Task On(ItemDeletedEvent @event)
    {
        await _itemRepository.DeleteAsync(@event.ItemId);
    }

    public async Task On(DiscountCreatedEvent @event)
    {
        var discount = new DiscountDb
        {
            OrderId = @event.AggregateId,
            LowerThreshold = @event.LowerThreshold,
            UpperThreshold = @event.UpperThreshold,
            Percentage = @event.Percentage
        };

        await _discountRepository.CreateAsync(discount);
    }

    public async Task On(DiscountUpdatedEvent @event)
    {
        DiscountDb? discount = await _discountRepository.GetByIdAsync(@event.DiscountId);

        if (discount is null) return;

        discount.LowerThreshold = @event.LowerThreshold;
        discount.UpperThreshold = @event.UpperThreshold;
        discount.Percentage = @event.Percentage;

        await _discountRepository.UpdateAsync(discount);
    }

    public async Task On(DiscountDeletedEvent @event)
    {
        await _discountRepository.DeleteAsync(@event.DiscountId);
    }
}