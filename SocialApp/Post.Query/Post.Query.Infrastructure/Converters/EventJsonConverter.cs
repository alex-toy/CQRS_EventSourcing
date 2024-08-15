using CQRS.Core.Events;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Posts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Post.Query.Infrastructure.Converters;

public class EventJsonConverter : JsonConverter<Event>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(Event));
    }

    public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
        }

        if (!doc.RootElement.TryGetProperty("Type", out var type))
        {
            throw new JsonException("Could not detect the Type discriminator property!");
        }

        var typeDiscriminator = type.GetString();
        var json = doc.RootElement.GetRawText();

        return typeDiscriminator switch
        {
            nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(PostUpdatedEvent) => JsonSerializer.Deserialize<PostUpdatedEvent>(json, options),
            nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(json, options),
            nameof(CommentCreatedEvent) => JsonSerializer.Deserialize<CommentCreatedEvent>(json, options),
            nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
            nameof(CommentDeletedEvent) => JsonSerializer.Deserialize<CommentDeletedEvent>(json, options),
            nameof(PostDeletedEvent) => JsonSerializer.Deserialize<PostDeletedEvent>(json, options),
            _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
        };
    }

    public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}