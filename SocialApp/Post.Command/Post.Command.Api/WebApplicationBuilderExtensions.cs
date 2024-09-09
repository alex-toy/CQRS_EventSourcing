using Confluent.Kafka;
using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using Post.Command.Api.Commands.Deliveries;
using Post.Command.Api.Commands.Discounts;
using Post.Command.Api.Commands.Orders;
using Post.Command.Api.Commands.Orders.Items;
using Post.Command.Api.Commands.Posts;
using Post.Command.Api.Commands.Posts.Comments;
using Post.Command.Api.Handlers.Discounts;
using Post.Command.Api.Handlers.Orders;
using Post.Command.Api.Handlers.Orders.Items;
using Post.Command.Api.Handlers.Posts;
using Post.Command.Api.Handlers.Posts.Comments;
using Post.Command.Domain;
using Post.Command.Infrastructure;
using Post.Command.Infrastructure.Configs;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Orders;
using Post.Common.Events.Orders.Discounts;
using Post.Common.Events.Orders.Items;
using Post.Common.Events.Posts;

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

        BsonClassMap.RegisterClassMap<ItemCreatedEvent>();
        BsonClassMap.RegisterClassMap<ItemUpdatedEvent>();
        BsonClassMap.RegisterClassMap<ItemDeletedEvent>();

        BsonClassMap.RegisterClassMap<DiscountCreatedEvent>();
        BsonClassMap.RegisterClassMap<DiscountUpdatedEvent>();
        BsonClassMap.RegisterClassMap<DiscountDeletedEvent>();
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

    public static void ConfigurePosts(this WebApplicationBuilder builder, CommandDispatcher dispatcher)
    {
        builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();
        builder.Services.AddScoped<IPostCommandHandler, PostCommandHandler>();
        builder.Services.AddScoped<ICommentCommandHandler, CommentCommandHandler>();

        IPostCommandHandler postCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostCommandHandler>();
        dispatcher.RegisterHandler<CreatePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdatePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<LikePostCommand>(postCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeletePostCommand>(postCommandHandler.HandleAsync);

        ICommentCommandHandler commentCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommentCommandHandler>();
        dispatcher.RegisterHandler<CreateCommentCommand>(commentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateCommentCommand>(commentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteCommentCommand>(commentCommandHandler.HandleAsync);
    }

    public static void ConfigureOrders(this WebApplicationBuilder builder, CommandDispatcher dispatcher)
    {
        builder.Services.AddScoped<IEventSourcingHandler<OrderAggregate>, EventSourcingHandler<OrderAggregate>>();
        builder.Services.AddScoped<IOrderCommandHandler, OrderCommandHandler>();
        builder.Services.AddScoped<IItemCommandHandler, ItemCommandHandler>();
        builder.Services.AddScoped<IDiscountCommandHandler, DiscountCommandHandler>();

        IOrderCommandHandler orderCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IOrderCommandHandler>();
        dispatcher.RegisterHandler<CreateOrderCommand>(orderCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateOrderCommand>(orderCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteOrderCommand>(orderCommandHandler.HandleAsync);

        IItemCommandHandler itemCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemCommandHandler>();
        dispatcher.RegisterHandler<CreateItemCommand>(itemCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateItemCommand>(itemCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteItemCommand>(itemCommandHandler.HandleAsync);

        IDiscountCommandHandler discountCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IDiscountCommandHandler>();
        dispatcher.RegisterHandler<CreateDiscountCommand>(discountCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateDiscountCommand>(discountCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeleteDiscountCommand>(discountCommandHandler.HandleAsync);
    }
}
