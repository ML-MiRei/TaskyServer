using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Configuration.AddJsonFile("config.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SenderService>();

app.Run();
