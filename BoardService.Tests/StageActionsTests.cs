using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace BoardService.Tests
{
    [TestClass]
    public class StageActionsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7191";
        public const string BOARD_ID = "84abf67f-7c8f-45d0-b64c-35ce6830d00c";
        public const int STAGE_ID = 1;

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Stages.StagesClient client = new Stages.StagesClient(channel);

            var dateStart = Timestamp.FromDateTime(DateTime.UtcNow);
            var dateEnd = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(1));

            var reply = await client.CreateStageAsync(new CreateStageRequest
            {
                BoardId = BOARD_ID,
                Name = "test",
                Queue = 0
            });

            Assert.IsNotNull(reply.StageId);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Stages.StagesClient client = new Stages.StagesClient(channel);

            var dateStart = Timestamp.FromDateTime(DateTime.UtcNow);
            var dateEnd = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(2));

            var reply = await client.UpdateStageAsync(new UpdateStageRequest
            {
                StageId = STAGE_ID,
                BoardId = BOARD_ID,
                Name = "test",
                Queue = 2
            });

            Assert.AreEqual(reply.StageId, STAGE_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Stages.StagesClient client = new Stages.StagesClient(channel);

            var reply = await client.GetStageAsync(new GetStageRequest { StageId = STAGE_ID });

            Console.WriteLine(reply);

            Assert.AreEqual(reply.StageId, STAGE_ID);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Stages.StagesClient client = new Stages.StagesClient(channel);

            var reply = await client.GetStagesByBoardIdAsync(new GetStagesRequest { BoardId = BOARD_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Stages);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Stages.StagesClient client = new Stages.StagesClient(channel);

            var reply = await client.DeleteStageAsync(new DeleteStageRequest { StageId = STAGE_ID });

            Assert.AreEqual(reply.StageId, STAGE_ID);
        }

    }
}
