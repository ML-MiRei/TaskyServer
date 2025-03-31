using Grpc.Net.Client;

namespace ProjectService.Tests
{
    [TestClass]
    public class ProjectsTests
    {

        public const string SERVICE_ADDRESS = "https://localhost:7052";
        public const string TITLE = "test title";
        public const string DETAILS = "test details";
        public const string USER_ID = "test user id";
        public const string USER_ID_2 = "test user id 2";
        public const string PROJECT_ID = "a76f02fe-afa1-4a80-b12b-055631ecdee3";
        public const string PROJECT_ID_2 = "f4ea9e3a-f3fc-4ba9-94e0-77e0c1607b67";


        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Projects.ProjectsClient client = new Projects.ProjectsClient(channel);

            var reply = await client.CreateProjectAsync(new CreateProjectRequest { Title = TITLE, Details = DETAILS, UserId = USER_ID });

            Assert.IsNotNull(reply.ProjectId);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Projects.ProjectsClient client = new Projects.ProjectsClient(channel);

            var newTitle = TITLE + " 2";

            var reply = await client.UpdateProjectAsync(new UpdateProjectRequest { Title = newTitle, Id = PROJECT_ID , Details = DETAILS});

            Assert.AreEqual(reply.ProjectId, PROJECT_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Projects.ProjectsClient client = new Projects.ProjectsClient(channel);

            var reply = await client.GetProjectAsync(new GetProjectRequest { ProjectId = PROJECT_ID});

            Console.WriteLine(reply);

            Assert.AreEqual(reply.Id, PROJECT_ID);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Projects.ProjectsClient client = new Projects.ProjectsClient(channel);

            var reply = await client.GetAllProjectsAsync(new GetAllProjectsRequest { UserId = USER_ID});

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Projects);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Projects.ProjectsClient client = new Projects.ProjectsClient(channel);

            var reply = await client.DeleteProjectAsync(new DeleteProjectRequest { ProjectId = PROJECT_ID_2});

            Assert.AreEqual(reply.ProjectId, PROJECT_ID);
        }
    }

}