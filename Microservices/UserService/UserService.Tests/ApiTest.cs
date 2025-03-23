using Grpc.Net.Client;

namespace UserService.Tests
{
    [TestClass]
    public class ApiTest
    {
        const string SERVICE_ADDRESS = "http://localhost:5244";
        const string EMAIL = "test@email.ru";
        readonly string TEST_ID = "b0d4ce5d-2757-4699-948c-cfa79ba00086";
        string _name = "test4";


        [TestMethod]
        public async Task CreateTest()
        {

            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Users.UsersClient client = new Users.UsersClient(channel);

            var reply = await client.CreateUserAsync(new CreateUserRequest { Email = EMAIL, Id = TEST_ID });

            Assert.AreEqual(reply.Id, TEST_ID);

        }

        [TestMethod]
        public async Task GetTest()
        {

            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Users.UsersClient client = new Users.UsersClient(channel);

            var reply = await client.GetUserAsync(new GetUserRequest { Id = TEST_ID });

            channel.Dispose();

            Assert.AreEqual(reply.Info.Email, EMAIL);


        }


        [TestMethod]
        public async Task GetAllTest()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Users.UsersClient client = new Users.UsersClient(channel);

            var request = new GetUsersRequest();
            request.Id.Add(TEST_ID);

            var reply = await client.GetUsersAsync(request);
            channel.Dispose();

            Assert.AreEqual(reply.Users[0].Name, _name);
        }



        [TestMethod]
        public async Task GetByNameTest()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Users.UsersClient client = new Users.UsersClient(channel);

            var reply = await client.FindByNameAsync(new FindByNameUsersRequest { Name = _name });
            channel.Dispose();

            Assert.AreEqual(reply.Users[0].Id, TEST_ID);
        }


        [TestMethod]
        public async Task UpdateTest()
        {
            _name = "test4";

            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Users.UsersClient client = new Users.UsersClient(channel);

            var reply = await client.UpdateUserAsync(new UpdateUserRequest { Info = new UserFullInfo { Name = _name, Id = TEST_ID, Email = EMAIL } });
            var reply2 = await client.GetUserAsync(new GetUserRequest { Id = TEST_ID });

            channel.Dispose();

            Assert.AreEqual(reply.Id, TEST_ID);
            Assert.AreEqual(reply2.Info.Name, _name);
        }

    }
}