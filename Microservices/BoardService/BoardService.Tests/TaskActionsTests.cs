using BoardService.API;
using BoardService.Core.Enums;
using Grpc.Net.Client;

namespace BoardService.Tests
{
    [TestClass]
    public class TaskActionsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7191";
        public const string BOARD_ID = "84abf67f-7c8f-45d0-b64c-35ce6830d00c";
        public const string TASK_ID = "84abf67f-7c8f-45d0-b64c-35ce6830d000";

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var request = new AddTasksRequest();
            request.TaskIds.Add(TASK_ID);
            request.BoardId = BOARD_ID;

            var reply = await client.AddTasksAsync(request);

            Assert.AreEqual(reply.BoardId, BOARD_ID);
        }

        [TestMethod]
        public async Task ChangeSprint()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var request = new ChangeSprintRequest();
            request.TaskId.Add(TASK_ID);
            request.BoardId = BOARD_ID;
            request.SprintId = 2;

            var reply = await client.ChangeSprintAsync(request);

            Assert.AreEqual(reply.SprintId, 2);
        }

        [TestMethod]
        public async Task ChangeStage()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.ChangeStageAsync(new ChangeStageRequest { TaskId = TASK_ID, StageId = 2});

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

        [TestMethod]
        public async Task GetByBoard()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.GetTasksByBoardAsync(new GetTasksByBoardRequest { BoardId = BOARD_ID });

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Tasks.Count == 1);
        }

        [TestMethod]
        public async Task GetBySprint()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.GetTasksBySprintAsync(new GetTasksBySprintRequest { SprintId = 2 });

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Tasks.Count == 1);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.DeleteTaskAsync(new DeleteTaskRequest { Id = TASK_ID });

            Assert.AreEqual(reply.Id, TASK_ID);
        }

    }
}
