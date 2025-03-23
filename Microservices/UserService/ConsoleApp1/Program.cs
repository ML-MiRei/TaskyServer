using Grpc.Net.Client;
using ConsoleApp1;
const string SERVICE_ADDRESS = "http://localhost:5244";
const string EMAIL = "test@email.ru";
 string TEST_ID = "b0d4ce5d-2757-4699-948c-cfa72ba90986";
string _name = "test4";



GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
Users.UsersClient client = new Users.UsersClient(channel);

var reply = await client.CreateUserAsync(new CreateUserRequest { Email = EMAIL, Id = TEST_ID });
var reply2 = await client.UpdateUserAsync(new UpdateUserRequest { Info = new UserFullInfo { Name = _name, Id = TEST_ID, Email = EMAIL } });

channel.Dispose();