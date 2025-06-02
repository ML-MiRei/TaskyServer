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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // ¬аш фронтенд URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // ≈сли используете куки/авторизацию
        });
});
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(443, listenOptions =>
//    {
//        listenOptions.UseHttps("/app/certs/certificate.pfx", "password");
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // явно указываем протоколы
//    });

//    serverOptions.ListenAnyIP(80, listenOptions =>
//    {
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//    });
//});


// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline
// 

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");



app.MapControllers();
//app.UseHttpsRedirection();


app.Run();