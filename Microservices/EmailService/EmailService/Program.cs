using EmailService.Models;
using EmailService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpSettings"));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(86, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SenderService>();

app.Run();
