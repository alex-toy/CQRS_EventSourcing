using Confluent.Kafka;
using CQRS.Core.Commands;
using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using Post.Command.Api.Commands.Comments;
using Post.Command.Api.Commands.Orders;
using Post.Command.Api.Commands.Posts;
using Post.Command.Api.Handlers.Comments;
using Post.Command.Api.Handlers.Orders;
using Post.Command.Api.Handlers.Posts;
using Post.Command.Domain;
using Post.Command.Infrastructure;
using Post.Command.Infrastructure.Configs;
using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Orders;
using Post.Common.Events.Posts;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();

builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();
builder.Services.AddScoped<IPostCommandHandler, PostCommandHandler>();
builder.Services.AddScoped<ICommentCommandHandler, CommentCommandHandler>();

builder.Services.AddScoped<IEventSourcingHandler<OrderAggregate>, EventSourcingHandler<OrderAggregate>>();
builder.Services.AddScoped<IOrderCommandHandler, OrderCommandHandler>();

// register command handler methods
var dispatcher = new CommandDispatcher();

IPostCommandHandler postCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostCommandHandler>();
dispatcher.RegisterHandler<CreatePostCommand>(postCommandHandler.HandleAsync);
dispatcher.RegisterHandler<UpdatePostCommand>(postCommandHandler.HandleAsync);
dispatcher.RegisterHandler<LikePostCommand>(postCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeletePostCommand>(postCommandHandler.HandleAsync);

ICommentCommandHandler commentCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommentCommandHandler>();
dispatcher.RegisterHandler<CreateCommentCommand>(commentCommandHandler.HandleAsync);
dispatcher.RegisterHandler<UpdateCommentCommand>(commentCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteCommentCommand>(commentCommandHandler.HandleAsync);

IOrderCommandHandler orderCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IOrderCommandHandler>();
dispatcher.RegisterHandler<CreateOrderCommand>(orderCommandHandler.HandleAsync);
dispatcher.RegisterHandler<UpdateOrderCommand>(orderCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteOrderCommand>(orderCommandHandler.HandleAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();