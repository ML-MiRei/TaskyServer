using UserService.API.Services;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Abstractions.Services;
using UserService.Application.Services;
using UserService.Infrastructure.Implementations.Repositories;
using UserService.Infrastructure.Implementations.Services;
using UserService.Infrastructure.Implementations.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IImageProvider, ImageProvider>();
builder.Services.AddSingleton<UserActionsService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.Configure<ConnectionsSettings>(builder.Configuration.GetSection(nameof(ConnectionsSettings)));

var app = builder.Build();

app.MapGrpcService<UsersService>();

app.Run();
