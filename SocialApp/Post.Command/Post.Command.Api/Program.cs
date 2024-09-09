using CQRS.Core.Commands;
using Post.Command.Api;
using Post.Command.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplicationBuilderExtensions.ConfigureBson();

// Add services to the container.
builder.ConfigureMongo();
builder.ConfigureEventStore();
CommandDispatcher dispatcher = new();

builder.ConfigurePosts(dispatcher);
builder.ConfigureOrders(dispatcher);
builder.ConfigureDeliveries(dispatcher);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();








WebApplication app = builder.Build();

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