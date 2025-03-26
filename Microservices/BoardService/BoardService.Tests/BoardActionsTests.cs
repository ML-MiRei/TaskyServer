using BoardService.Core.Enums;
using Grpc.Net.Client;

namespace BoardService.Tests
{
    [TestClass]
    public class BoardActionsTests
    {

        public const string SERVICE_ADDRESS = "https://localhost:7191";
        public const string BOARD_ID = "4e6aa12e-7dbb-430b-b270-f30026fbcedb";
        public const string BOARD_ID_2 = "84abf67f-7c8f-45d0-b64c-35ce6830d00c";

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.CreateBoardAsync(new CreateBoardRequest { Title = "title", Type = (int)BoardType.Kanban});

            Assert.IsNotNull(reply.Id);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.UpdateBoardAsync(new UpdateBoardRequest { Title = "title2", Id = BOARD_ID});

            Assert.AreEqual(reply.Id, BOARD_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.GetBoardAsync(new GetBoardRequest { Id = BOARD_ID});

            Console.WriteLine(reply);

            Assert.AreEqual(reply.Id, BOARD_ID);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var request = new GetBoardsRequest();
            request.Id.AddRange([BOARD_ID, BOARD_ID_2]);

            var reply = await client.GetBoardsAsync(request);

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Boards.Count == request.Id.Count);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Boards.BoardsClient client = new Boards.BoardsClient(channel);

            var reply = await client.DeleteBoardAsync(new DeleteBoardRequest {Id = BOARD_ID });

            Assert.AreEqual(reply.Id, BOARD_ID);
        }

    }
}
