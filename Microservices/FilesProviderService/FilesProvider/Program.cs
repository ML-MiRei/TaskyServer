using Amazon;
using Amazon.S3;
using FilesProvider.Services;
using FilesProvider.Settings;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection(nameof(S3Settings)));
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
    var config = new AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region),
        ServiceURL = s3Settings.Endpoint
    };

    return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, config);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(88, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    options.ListenAnyIP(80);
});

var app = builder.Build();

app.MapGrpcService<FilesService>();

app.Run();
