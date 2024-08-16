using Post.Query.Api;
using Post.Query.Infrastructure.Consumers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureDatabase();
builder.ConfigurePosts();
builder.ConfigureOrders();


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
