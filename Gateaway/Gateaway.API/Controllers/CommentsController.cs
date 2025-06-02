using Gateaway.Core.Common;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.Comments;
using Getaway.Core.Contracts.Stages;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/tasks/{taskId}/[controller]")]
    [Authorize]
    public class CommentsController(Connections connections) : ControllerBase
    {

        [HttpPost()]
        public async Task<ActionResult<CreateCommentReply>> CreateComment([FromBody] CreateCommentRequest createCommentRequest)
        {
            try
            {
                var reply = await connections.CommentsServiceClient.CreateCommentAsync(createCommentRequest);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult<DeleteCommentReply>> DeleteStage(int commentId)
        {
            try
            {
                var reply = await connections.CommentsServiceClient.DeleteCommentAsync(new DeleteCommentRequest { CommentId = commentId });
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult<UpdateCommentReply>> UpdateComment([FromBody] UpdateCommentRequest updateCommentRequest)
        {
            try
            {
                var reply = await connections.CommentsServiceClient.UpdateCommentAsync(updateCommentRequest);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet()]
        public async Task<ActionResult<List<StageInfo>>> GetStages(string taskId)
        {
            try
            {
                var reply = await connections.CommentsServiceClient.GetAllCommentsByTaskAsync(new GetAllCommentsByTaskRequest
                {
                    TaskId = taskId
                });
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}
