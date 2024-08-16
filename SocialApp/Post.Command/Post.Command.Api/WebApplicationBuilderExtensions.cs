﻿using Confluent.Kafka;
using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using Post.Command.Infrastructure.Configs;
using Post.Command.Infrastructure;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Orders;
using Post.Common.Events.Posts;
using Post.Command.Api.Commands.Comments;
using Post.Command.Api.Commands.Posts;
using Post.Command.Api.Handlers.Comments;
using Post.Command.Api.Handlers.Posts;
using Post.Command.Domain;
using CQRS.Core.Commands;
using Post.Command.Api.Handlers.Orders;
using Post.Command.Api.Commands.Orders;

namespace Post.Command.Api;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBson()
    {
        BsonClassMap.RegisterClassMap<Event>();

        BsonClassMap.RegisterClassMap<PostCreatedEvent>();
        BsonClassMap.RegisterClassMap<PostUpdatedEvent>();
        BsonClassMap.RegisterClassMap<PostLikedEvent>();
        BsonClassMap.RegisterClassMap<PostDeletedEvent>();

        BsonClassMap.RegisterClassMap<CommentCreatedEvent>();
        BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
        BsonClassMap.RegisterClassMap<CommentDeletedEvent>();

        BsonClassMap.RegisterClassMap<OrderCreatedEvent>();
        BsonClassMap.RegisterClassMap<OrderUpdatedEvent>();
        BsonClassMap.RegisterClassMap<OrderDeletedEvent>();
    }

    public static void ConfigureMongo(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
        builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
    }

    public static void ConfigureEventStore(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        builder.Services.AddScoped<IEventProducer, EventProducer>();
        builder.Services.AddScoped<IEventStore, EventStore>();
    }

    public static void ConfigurePosts(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();
        builder.Services.AddScoped<IPostCommandHandler, PostCommandHandler>();
        builder.Services.AddScoped<ICommentCommandHandler, CommentCommandHandler>();

        CommandDispatcher dispatcher = new();

        IPostCommandHandler postCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostCommandHandler>();
        dispatcher.RegisterHandler<CreatePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdatePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<LikePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeletePostCommand>(postCommandHandler.HandleAsync);

        ICommentCommandHandler commentCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommentCommandHandler>();
        dispatcher.RegisterHandler<CreateCommentCommand>(commentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateCommentCommand>(commentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteCommentCommand>(commentCommandHandler.HandleAsync);

        builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
    }

    public static void ConfigureOrders(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventSourcingHandler<OrderAggregate>, EventSourcingHandler<OrderAggregate>>();
        builder.Services.AddScoped<IOrderCommandHandler, OrderCommandHandler>();

        CommandDispatcher dispatcher = new();

        IOrderCommandHandler orderCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IOrderCommandHandler>();
        dispatcher.RegisterHandler<CreateOrderCommand>(orderCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateOrderCommand>(orderCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteOrderCommand>(orderCommandHandler.HandleAsync);

        builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
    }
}
