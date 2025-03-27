using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace TaskService.Tests
{
    [TestClass]
    public class ExecutionsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7162";
        public const string USER_ID = "test-user-id";
        public const string USER_ID_2 = "test-user-id-2";
        public const string TASK_ID = "17d03f2c-8207-459c-8ed2-eaaf5fe21800";

        [TestMethod]
        public async Task Add()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.AddExecutorAsync(new AddExecutorRequest { ExecutorId = USER_ID_2, TaskId = TASK_ID });

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.DeleteExecutorAsync(new DeleteExecutorRequest { TaskId = TASK_ID , ExecutorId = USER_ID});

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }        
        
        
        [TestMethod]
        public async Task SetFinishedExecutions()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.SetFinishedExecutionsAsync(new SetFinishedExecutionsRequest{ TaskId = TASK_ID });

            Assert.AreEqual(reply.TaskId, TASK_ID);
        }

        [TestMethod]
        public async Task GetExecutors()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.GetExecutorsAsync(new GetExecutorsRequest { TaskId = TASK_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.ExecutorIds);
        }

        [TestMethod]
        public async Task GetHistoryByTask()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.GetHistoryByTaskAsync(new GetHistoryByTaskRequest { TaskId = TASK_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Executions);
        }


        [TestMethod]
        public async Task GetHistoryByUser()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.GetHistoryByUserAsync(new GetHistoryByUserRequest { ExecutorId = USER_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Executions);
        }

        [TestMethod]
        public async Task GetStateExecutionsByUser()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Executions.ExecutionsClient client = new Executions.ExecutionsClient(channel);

            var reply = await client.GetStateExecutionsByUserAsync(new GetStateExecutionsByUserRequest { ExecutorId = USER_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Executions);
        }

    }
}
