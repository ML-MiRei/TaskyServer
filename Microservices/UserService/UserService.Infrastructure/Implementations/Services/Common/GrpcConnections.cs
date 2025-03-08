using Grpc.Net.Client;
using UserService.Infrastructure.Contracts;

namespace UserService.Infrastructure.Implementations.Services.Common
{
    public class GrpcConnections : IDisposable
    {
        public FilesProvider.FilesProviderClient FilesProviderClient { get; set; }
        private GrpcChannel _grpcChannel;


        public GrpcConnections(string filesProviderAddress)
        {
            _grpcChannel = GrpcChannel.ForAddress(filesProviderAddress);
            FilesProviderClient = new FilesProvider.FilesProviderClient(_grpcChannel);
        }

        public void Dispose()
        {
            _grpcChannel.Dispose();
        }
    }
}
