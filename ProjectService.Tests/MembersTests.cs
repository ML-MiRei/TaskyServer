using Grpc.Net.Client;

namespace ProjectService.Tests
{
    [TestClass]
    public class MembersTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7052";
        public const string USER_ID = "test user id";
        public const string USER_ID_2 = "test user id 2";
        public const string PROJECT_ID = "a76f02fe-afa1-4a80-b12b-055631ecdee3";


        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Members.MembersClient client = new Members.MembersClient(channel);

            var reply = await client.AddMemberAsync(new AddMemberRequest { UserId = USER_ID_2, ProjectId = PROJECT_ID });

            Assert.IsNotNull(reply.ProjectId);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Members.MembersClient client = new Members.MembersClient(channel);

            var reply = await client.ChangeMemberRoleAsync(new ChangeMemberRoleRequest { RoleId = 2, ProjectId = PROJECT_ID, UserId = USER_ID_2 });

            Assert.AreEqual(reply.ProjectId, PROJECT_ID);
        }

        [TestMethod]
        public async Task Get()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Members.MembersClient client = new Members.MembersClient(channel);

            var reply = await client.GetMemberRoleAsync(new GetMemberRoleRequest { ProjectId = PROJECT_ID, UserId = USER_ID_2 });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.RoleId);
        }

        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Members.MembersClient client = new Members.MembersClient(channel);

            var reply = await client.GetMembersAsync(new GetMembersRequest { ProjectId = PROJECT_ID });

            Console.WriteLine(reply);

            Assert.IsNotNull(reply.Members);
        }


        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Members.MembersClient client = new Members.MembersClient(channel);

            var reply = await client.DeleteMemberAsync(new DeleteMemberRequest { ProjectId = PROJECT_ID, UserId = USER_ID_2 });

            Assert.AreEqual(reply.ProjectId, PROJECT_ID);
        }
    }
}
