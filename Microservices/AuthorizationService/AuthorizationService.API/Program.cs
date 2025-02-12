using AuthorizationService.Applicaion.Abstractions.Repositories;
using AuthorizationService.Applicaion.Abstractions.Services;
using AuthorizationService.Applicaion.DTO;
using AuthorizationService.Applicaion.Services;
using AuthorizationService.Infrastructure.Implementations.Services;
using AuthorizationService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUsersRepository, UserRepository>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.



app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();


