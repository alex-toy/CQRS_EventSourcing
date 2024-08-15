﻿namespace CQRS.Core.Events;

public interface IEventProducer
{
    Task ProduceAsync<T>(string topic, T @event) where T : Event;
}