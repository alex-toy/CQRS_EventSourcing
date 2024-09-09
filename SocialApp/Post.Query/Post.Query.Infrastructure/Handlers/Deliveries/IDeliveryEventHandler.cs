﻿using Post.Common.Events.Deliveries;

namespace Post.Query.Infrastructure.Handlers.Deliveries;

public interface IDeliveryEventHandler : IEventHandler
{
    Task On(DeliveryCreatedEvent @event);
}