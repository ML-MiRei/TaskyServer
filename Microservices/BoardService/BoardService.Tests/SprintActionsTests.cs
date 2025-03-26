using BoardService.Core.Enums;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace BoardService.Tests
{
    [TestClass]
    public class SprintActionsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7191";
        public const string BOARD_ID = "84abf67f-7c8f-45d0-b64c-35ce6830d00c";
        public const int SPRINT_ID = 1;

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Sprints.SprintsClient client = new Sprints.SprintsClient(channel);

            var dateStart = Timestamp.FromDateTime(DateTime.UtcNow);
            var dateEnd = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(1));

            var reply = await client.CreateSprintAsync(new CreateSprintRequest { BoardId = BOARD_ID, DateEnd = dateEnd, DateStart = dateStart});

            Assert.IsNotNull(reply.Id);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Sprints.SprintsClient client = new Sprints.SprintsClient(channel);

            var dateStart = Timestamp.FromDateTime(DateTime.UtcNow);
            var dateEnd = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(2));

            var reply = await client.UpdateSprintAsync(new UpdateSprintRequest {DateStart = dateStart, DateEnd = dateEnd, Id = SPRINT_ID });

            Assert.AreEqual(reply.Id, SPRINT_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Sprints.SprintsClient client = new Sprints.SprintsClient(channel);

            var reply = await client.GetSprintAsync(new GetSprintRequest { Id = SPRINT_ID });

            Console.WriteLine(reply);

            Assert.AreEqual(reply.Id, SPRINT_ID);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Sprints.SprintsClient client = new Sprints.SprintsClient(channel);

            var reply = await client.GetSprintsAsync(new GetSprintsRequest { BoardId = BOARD_ID});

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Sprints.Count == 1);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Sprints.SprintsClient client = new Sprints.SprintsClient(channel);

            var reply = await client.DeleteSprintAsync(new DeleteSprintRequest { Id = SPRINT_ID });

            Assert.AreEqual(reply.Id, SPRINT_ID);
        }

    }
}
