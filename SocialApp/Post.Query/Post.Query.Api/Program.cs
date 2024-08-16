using Confluent.Kafka;
using CQRS.Core.Events;
using Post.Query.Api;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.Handlers.Orders;
using Post.Query.Infrastructure.Handlers.Posts;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureDatabase();
builder.ConfigurePosts();
//builder.ConfigureOrders();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer<IPostEventHandler>>();
//builder.Services.AddScoped<IEventConsumer, EventConsumer<IOrderEventHandler>>();


builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();

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
