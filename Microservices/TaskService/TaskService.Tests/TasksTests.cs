using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.VisualBasic;

namespace TaskService.Tests
{
    [TestClass]
    public class TasksTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7162";
        public const string TITLE = "test title";
        public readonly Timestamp DATE_END = Timestamp.FromDateTime(DateTime.UtcNow);
        public const string PROJECT_ID = "test-project-id";
        public const string USER_ID = "test-user-id";
        public const string TASK_ID = "42d5765a-28cb-46be-9eed-7f4783c34063";
        public const string DETAILS = "test details";

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.CreateTaskAsync(new CreateTaskRequest { Title = TITLE, DateEnd = DATE_END, Details = DETAILS, ProjectId = PROJECT_ID });

            Assert.IsNotNull(reply.TaskId);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var newTitle = "new " + TITLE;

            var reply = await client.UpdateTaskAsync(new UpdateTaskRequest { TaskId = TASK_ID, DateEnd = DATE_END, Details = DETAILS, Title = newTitle});

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.GetTaskAsync(new GetTaskRequest { TaskId = TASK_ID});

            Console.WriteLine(reply);

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

        [TestMethod]
        public async Task GetAllByProject()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.GetAllTasksByProjectAsync(new GetAllTasksByProjectRequest { ProjectId = PROJECT_ID });

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Tasks.Count == 8);
        }


        [TestMethod]
        public async Task GetAllByUser()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.GetAllTasksByUserAsync(new GetAllTasksByUserRequest { UserId = USER_ID });

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Tasks.Count == 0);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Tasks.TasksClient client = new Tasks.TasksClient(channel);

            var reply = await client.DeleteTaskAsync(new DeleteTaskRequest { TaskId = TASK_ID });

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

    }
}