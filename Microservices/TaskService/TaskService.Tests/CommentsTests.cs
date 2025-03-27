using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.VisualBasic;

namespace TaskService.Tests
{
    [TestClass]
    public class CommentsTests
    {
        public const string SERVICE_ADDRESS = "https://localhost:7162";
        public const string TEXT = "test text";
        public const string USER_ID = "test-user-id";
        public const string TASK_ID = "31bd3f87-1036-40f6-8b9d-f06bfe703d94";
        public const int COMMENT_ID = 2;

        [TestMethod]
        public async Task Create()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var reply = await client.CreateCommentAsync(new CreateCommentRequest { CreatorId = USER_ID, TaskId = TASK_ID, Text = TEXT });

            Assert.IsNotNull(reply.CommentId);
        }

        [TestMethod]
        public async Task Update()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var newText = "new " + TEXT;

            var reply = await client.UpdateCommentAsync(new UpdateCommentRequest { CreatorId = USER_ID, CommentId = COMMENT_ID, Text = TEXT });

            Assert.AreEqual(reply.CommentId, COMMENT_ID);
        }

        [TestMethod]
        public async Task UpdateWithDelay()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var reply = await client.CreateCommentAsync(new CreateCommentRequest { CreatorId = USER_ID, TaskId = TASK_ID, Text = TEXT });

            var newText = "new " + TEXT;
            Thread.Sleep(60000 * 3 + 5);

            try
            {
                var reply2 = await client.UpdateCommentAsync(new UpdateCommentRequest { CreatorId = USER_ID, CommentId = reply.CommentId, Text = TEXT });
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual(ex.StatusCode, StatusCode.Unavailable);
            }
        }


        [TestMethod]
        public async Task GetAll()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var reply = await client.GetAllCommentsByTaskAsync(new GetAllCommentsByTaskRequest { TaskId = TASK_ID });

            Console.WriteLine(reply);

            Assert.IsTrue(reply.Comments.Count == 1);
        }



        [TestMethod]
        public async Task Delete()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var reply = await client.DeleteCommentAsync(new DeleteCommentRequest { CommentId = COMMENT_ID });

            Assert.AreEqual(reply.CommentId, COMMENT_ID);
        }

        [TestMethod]
        public async Task DeleteWithDelay()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(SERVICE_ADDRESS);
            Comments.CommentsClient client = new Comments.CommentsClient(channel);

            var reply = await client.CreateCommentAsync(new CreateCommentRequest { CreatorId = USER_ID, TaskId = TASK_ID, Text = TEXT });

            Thread.Sleep(60000 * 3 + 5);

            try
            {
                var reply2 = await client.DeleteCommentAsync(new DeleteCommentRequest { CommentId = reply.CommentId });
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual(ex.StatusCode, StatusCode.Unavailable);
            }
        }

    }
}