using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.Services;
using AuthenticationService.Infrastructure.Database;
using AuthenticationService.Infrastructure.Implementations.Repositories;
using AuthenticationService.Infrastructure.Implementations.Services;
using AuthenticationService.Infrastructure.Implementations.Services.Models;
using AuthenticationService.Infrastructure.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IAuthDataRepository, AuthDataRepository>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();
builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("ConnectionString:AuthDb"));
builder.Services.Configure<VerificationOptions>(builder.Configuration.GetSection("VerificationSettings"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddTransient<AuthDbContext>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddLogging();
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


