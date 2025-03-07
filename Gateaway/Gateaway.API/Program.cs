using Gateaway.API.Extensions;
using Gateaway.Core.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection("ConnectionStrings"));


// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline
// 

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
