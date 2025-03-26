using Google.Protobuf;
using Grpc.Net.Client;

namespace FilesProviderTests
{
    [TestClass]
    public class FilesLoaderUnitTest
    {
        private const string SERVICE_ADDRESS = "http://localhost:5051";
        private const string KEY = "key";




        [TestMethod]
        public async Task UploadAsync()
        {

            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            FilesProvider.FilesProviderClient client = new FilesProvider.FilesProviderClient(channel);

            FileStream fileStream = new FileStream("C:\\Users\\feyri\\source\\repos\\TaskyServer\\FilesProviderTests\\TestData\\test.jpeg", FileMode.Open);

            var reply = await client.UploadFileAsync(new UploadFileRequest { Key = KEY, ChunkData = ByteString.FromStream(fileStream), FileType = "jpeg" });

            channel.Dispose();
            fileStream.Close();

            Assert.AreEqual(KEY, reply.Key);
        }

        [TestMethod]
        public async Task GetAsync()
        {

            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            FilesProvider.FilesProviderClient client = new FilesProvider.FilesProviderClient(channel);
            FileStream fileStream = new FileStream("C:\\Users\\feyri\\source\\repos\\TaskyServer\\FilesProviderTests\\TestData\\test.jpeg", FileMode.Open);

            var reply = await client.GetFileAsync(new GetFileRequest { Key = KEY });

            channel.Dispose();

            Assert.AreEqual(reply.FileType, "jpeg");
            Assert.AreEqual(reply.ChunkData, ByteString.FromStream(fileStream));

            fileStream.Close();
        }


        [TestMethod]
        public async Task DeleteAsync()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            FilesProvider.FilesProviderClient client = new FilesProvider.FilesProviderClient(channel);

            var reply = await client.DeleteFileAsync(new DeleteFileRequest { Key = KEY });

            channel.Dispose();

            Assert.AreEqual(KEY, reply.Key);
        }
    }
}