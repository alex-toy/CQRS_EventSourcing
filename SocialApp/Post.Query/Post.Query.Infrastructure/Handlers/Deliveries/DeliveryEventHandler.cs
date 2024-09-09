﻿using Post.Common.Events.Deliveries;
using Post.Query.Domain.Entities.Deliveries;
using Post.Query.Domain.Repositories.Orders;
using Post.Query.Infrastructure.Repositories.Deliveries;

namespace Post.Query.Infrastructure.Handlers.Deliveries;

public class DeliveryEventHandler : IDeliveryEventHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveryEventHandler(IOrderRepository orderRepository, IDeliveryRepository deliveryRepository)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
    }

    public async Task On(DeliveryCreatedEvent @event)
    {
        var delivery = new DeliveryDb
        {
            DeliveryId = @event.AggregateId,
            DriverName = @event.DriverName,
            CreatedAt = @event.CreatedAt
        };

        await _deliveryRepository.CreateAsync(delivery);
    }
}
