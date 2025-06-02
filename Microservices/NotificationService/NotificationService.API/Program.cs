using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using NotificationService.API.SignalR;
using NotificationService.API.SignalR.Hubs;
using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.Implementations.Repositories;
using NotificationService.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<NotificationDbContext>();
builder.Services.AddSingleton<INotificationsRepository, NotificationsRepository>();
builder.Services.AddSingleton<NotificationsService>();
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<IUserIdProvider, UserProvider>();
builder.Services.AddSingleton<INotificationSender, NotificationHub>();

builder.Services.AddGrpc();
builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(87, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.MapGrpcService<NotificationService.API.Services.NotificationsService>();
app.MapHub<NotificationHub>("\\notifications");

app.Run();
