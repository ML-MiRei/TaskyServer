using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Application.Abstractions.Services;
using UserService.Core.Common;
using UserService.Infrastructure.Contracts;
using UserService.Infrastructure.Implementations.Services.Common;
using UserService.Infrastructure.Implementations.Services.Models;

namespace UserService.Infrastructure.Implementations.Services
{
    public class ImageProvider : IImageProvider
    {
        private static ILogger<ImageProvider> _logger;
        private GrpcConnections _connections;
        private FilesProvider.FilesProviderClient _client;

        public ImageProvider(ILogger<ImageProvider> logger, IOptions<ConnectionsSettings> settings)
        {
            //_logger = logger;
            //_connections = new GrpcConnections(settings.Value.FilesProviderServiceAddress);
            //_client = _connections.FilesProviderClient;
        }

        public async Task<Result<string>> Upload(string key, byte[] image, string fileExtension)
        {
            var resFactory = new ResultFactory<string>();

            if (string.IsNullOrEmpty(key))
            {
                resFactory.AddError("Ключ не может быть пустым");
                return resFactory.Create();
            }

            try
            {
                var response = await _client.UploadFileAsync(new UploadFileRequest
                {
                    ChunkData = ByteString.CopyFrom(image),
                    Key = key,
                    FileType = Path.GetExtension(fileExtension)
                });

                resFactory.SetResult(response.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                resFactory.AddError(ex.Message);
            }

            return resFactory.Create();
        }


        public async Task<Result<(byte[], string)>> GetImage(string key)
        {

            var resFactory = new ResultFactory<(byte[], string)> ();

            if (string.IsNullOrEmpty(key))
            {
                resFactory.AddError("Ключ не может быть пустым");
                return resFactory.Create();
            }

            try
            {
                var response = await _client.GetFileAsync(new GetFileRequest
                {
                    Key = key
                });

                resFactory.SetResult((response.ChunkData.ToByteArray(), response.FileType));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                resFactory.AddError(ex.Message);
            }

            return resFactory.Create();
        }
    }
}
