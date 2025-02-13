using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Applicaion.Services;
using AuthenticationService.Infrastructure.Database;
using AuthenticationService.Infrastructure.Implementations.Services;
using AuthenticationService.Infrastructure.Implementations.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddTransient<IAuthDataRepository, AuthDataRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AuthDbContext>();

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


