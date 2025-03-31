using BoardService.Tests;
using Grpc.Net.Client;

namespace ProjectService.Tests
{
    [TestClass]
    public class BoardsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7052";
        public const string BOARD_ID = "test user id";
        public const string PROJECT_ID = "a76f02fe-afa1-4a80-b12b-055631ecdee3";


        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.AddBoardAsync(new AddBoardRequest { BoardId = BOARD_ID, ProjectId = PROJECT_ID });

            Assert.IsNotNull(reply.BoardId);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.GetAllBoardsAsync(new GetAllBoardsRequest { ProjectId = PROJECT_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Boards);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.DeleteBoardAsync(new DeleteBoardRequest { ProjectId = PROJECT_ID, BoardId = BOARD_ID });

            Assert.AreEqual(reply.BoardId, BOARD_ID);
        }
    }
}
