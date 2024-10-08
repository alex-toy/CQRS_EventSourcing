﻿using CQRS.Core.Domain;

namespace CQRS.Core.Events;

public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<T> GetByIdAsync(Guid aggregateId);
    Task RepublishEventsAsync();
}