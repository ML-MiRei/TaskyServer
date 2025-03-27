using TaskService.API.Services;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Services;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Implementations.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:TasksDb"));
builder.Services.AddDbContext<TasksDbContext>();
builder.Services.AddScoped<ExecutionManager>();
builder.Services.AddScoped<CommentsManager>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IExecutionsRepository, ExecutionsRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<TasksService>();
app.MapGrpcService<CommentsService>();
app.MapGrpcService<ExecutionsService>();

app.Run();
