var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<IRabbitPublisher, RabbitPublisher>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();