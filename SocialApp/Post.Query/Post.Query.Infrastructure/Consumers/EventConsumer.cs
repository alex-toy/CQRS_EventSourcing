using Confluent.Kafka;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using System.Reflection;
using System.Text.Json;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _config;
    private readonly IEnumerable<IEventHandler> _eventHandlers;

    public EventConsumer(IOptions<ConsumerConfig> config, IEnumerable<IEventHandler> eventHandlers)
    {
        _config = config.Value;
        _eventHandlers = eventHandlers;
    }

    public void Consume(string topic)
    {
        using IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            ConsumeResult<string, string> consumeResult = consumer.Consume();

            if (consumeResult?.Message is null) continue;

            JsonSerializerOptions options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
            Event? @event = JsonSerializer.Deserialize<Event>(consumeResult.Message.Value, options);

            int index = 0;
            MethodInfo? handlerMethod = null;
            foreach(IEventHandler eventHandler in _eventHandlers)
            {
                handlerMethod = eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });
                if (handlerMethod is not null) break;
                index++;
            }

            if (handlerMethod is null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
            }

            handlerMethod.Invoke(_eventHandlers.ToArray()[index], new object[] { @event });
            consumer.Commit(consumeResult);
        }
    }
}