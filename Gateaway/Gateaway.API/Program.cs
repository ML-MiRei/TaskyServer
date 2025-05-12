using Gateaway.API.Extensions;
using Gateaway.Core.Common;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IUserIdProvider, UserPro>();

builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddControllers();
builder.Services.AddSingleton<Connections>();
builder.Services.AddApiAuthentication(builder.Configuration);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.UseHttps("/app/certs/certificate.pfx", "password");
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // явно указываем протоколы
    });
});


// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline
// 

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//app.UseHttpsRedirection();


app.Run();