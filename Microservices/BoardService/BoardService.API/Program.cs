using BoardService.API.Services;
using BoardService.Application.Abstractions.Repositories;
using BoardService.Application.Services;
using BoardService.Infrastructure.Database;
using BoardService.Infrastructure.Implementations.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:BoardDb"));
builder.Services.AddDbContext<BoardDbContext>();
builder.Services.AddScoped<StartDataLoader>();
builder.Services.AddScoped<IBoardsRepository, BoardsRepository>();
builder.Services.AddScoped<ISprintsRepository, SprintsRepository>();
builder.Services.AddScoped<IStagesRepository, StagesRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(84, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    options.ListenAnyIP(80);
});


var app = builder.Build();

app.MapGrpcService<BoardsService>();
app.MapGrpcService<SprintsService>();
app.MapGrpcService<TasksService>();
app.MapGrpcService<StagesService>();

app.Run();
