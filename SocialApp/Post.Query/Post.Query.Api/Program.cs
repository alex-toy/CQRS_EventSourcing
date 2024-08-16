using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Queries;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Handlers.Orders;
using Post.Query.Api.Handlers.Posts;
using Post.Query.Api.Queries.Orders;
using Post.Query.Api.Queries.Posts;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.Data;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string SQLConnectionString = builder.Configuration.GetConnectionString("SqlServer")!;
Action<DbContextOptionsBuilder> configureDbContext = o => o.UseLazyLoadingProxies().UseSqlServer(SQLConnectionString);
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

// create database and tables
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPostQueryHandler, PostQueryHandler>();
builder.Services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

// register query handler methods
QueryDispatcher<PostDb> dispatcher = new();
IPostQueryHandler queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostQueryHandler>();
dispatcher.RegisterHandler<GetAllPostsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<GetPostByIdQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<GetPostsByAuthorQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<GetPostsWithCommentsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<GetPostsWithLikesQuery>(queryHandler.HandleAsync);
builder.Services.AddSingleton<IQueryDispatcher<PostDb>>(_ => dispatcher);

QueryDispatcher<OrderDb> orderDispatcher = new();
IOrderQueryHandler orderQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IOrderQueryHandler>();
orderDispatcher.RegisterHandler<GetAllOrdersQuery>(orderQueryHandler.HandleAsync);
orderDispatcher.RegisterHandler<GetOrderByIdQuery>(orderQueryHandler.HandleAsync);
builder.Services.AddSingleton<IQueryDispatcher<OrderDb>>(_ => orderDispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
