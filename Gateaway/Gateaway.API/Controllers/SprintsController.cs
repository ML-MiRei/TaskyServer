using Gateaway.Core.Common;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.Sprints;
using Getaway.Core.Contracts.Stages;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SprintsController(Connections connections) : ControllerBase
    {

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateSprintReply>> AddSprint([FromBody] CreateSprintRequest createSprintRequest)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.CreateSprintAsync(createSprintRequest);
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

        [HttpDelete("{sprintId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteSprintReply>> DeleteSprint(int sprintId)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.DeleteSprintAsync(new DeleteSprintRequest { Id = sprintId });
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

        [HttpPut("{sprintId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UpdateSprintReply>> UpdateSprint([FromBody] UpdateSprintRequest updateSprintRequest)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.UpdateSprintAsync(updateSprintRequest);
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

        [HttpGet("board={boardId}")]
        public async Task<ActionResult<GetSprintsReply>> GetSprints(string boardId)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.GetSprintsAsync(new GetSprintsRequest { BoardId = boardId });
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

        [HttpGet("{sprintId}")]
        public async Task<ActionResult<GetSprintReply>> GetSprint(int sprintId)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.GetSprintAsync(new GetSprintRequest { Id = sprintId });
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
