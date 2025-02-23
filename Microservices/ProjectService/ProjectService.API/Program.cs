using ProjectService.API.Services;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Implementations.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ProjectsDbContext>();
builder.Services.AddSingleton<IProjectsRepository, ProjectsRepository>();
builder.Services.AddSingleton<IMembersRepository, MembersRepository>();
builder.Services.AddSingleton<IProjectTasksRepository, ProjectTaskRepository>();
builder.Services.AddSingleton<ISprintsRepository, SprintsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProjectsService>();
app.MapGrpcService<SprintsService>();
app.MapGrpcService<MembersService>();
app.MapGrpcService<ProjectTasksService>();

app.Run();
