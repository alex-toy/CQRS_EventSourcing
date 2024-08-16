using Post.Command.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplicationBuilderExtensions.ConfigureBson();

// Add services to the container.
builder.ConfigureMongo();
builder.ConfigureEventStore();
//builder.ConfigurePosts();
builder.ConfigureOrders();

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