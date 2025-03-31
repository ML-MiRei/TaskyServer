using ProjectService.API.Services;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Implementations.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:ProjectDb"));
builder.Services.AddDbContext<ProjectsDbContext>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<IBoardsRepository, BoardsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProjectsService>();
app.MapGrpcService<BoardsService>();
app.MapGrpcService<MembersService>();

app.Run();
