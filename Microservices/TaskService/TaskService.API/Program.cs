using Microsoft.AspNetCore.Server.Kestrel.Core;
using Quartz;
using TaskService.API.Services;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Abstractions.Services;
using TaskService.Application.Services;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Implementations.Repositories;
using TaskService.Infrastructure.Implementations.Services;
using TaskService.Infrastructure.Implementations.Services.Scheduler;
using TaskService.Infrastructure.Kafka;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:TasksDb"));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddDbContext<TasksDbContext>();
builder.Services.AddScoped<ExecutionManager>();
builder.Services.AddScoped<CommentsManager>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IExecutionsRepository, ExecutionsRepository>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddScoped<IAutoNotificationsService, AutoNotificationsService>();
builder.Services.AddSingleton<IJob, DataJob>();
builder.Services.AddSingleton<ScheduleEvents, DataJob>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(85, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});


var app = builder.Build();

DataScheduler.Start(app.Services);

app.MapGrpcService<TasksService>();
app.MapGrpcService<CommentsService>();
app.MapGrpcService<ExecutionsService>();

app.Run();
