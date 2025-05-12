using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;
using ProjectService.API.Services;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Application.Abstractions.Services;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Implementations.Repositories;
using ProjectService.Infrastructure.Implementations.Services;
using ProjectService.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:ProjectDb"));
builder.Services.AddDbContext<ProjectsDbContext>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<IBoardsRepository, BoardsRepository>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddLogging(logging =>
 {
     logging.AddConsole();
 });


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(82, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<ProjectsService>();
app.MapGrpcService<BoardsService>();
app.MapGrpcService<MembersService>();

app.Run();


