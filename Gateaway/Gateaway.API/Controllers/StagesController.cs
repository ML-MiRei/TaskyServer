using Gateaway.Core.Common;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.Stages;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StagesController(Connections connections) : ControllerBase
    {

        [HttpPost()]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateStageReply>> AddStage([FromBody] CreateStageRequest createStageRequest)
        {
            try
            {
                var reply = await connections.StagesServiceClient.CreateStageAsync(createStageRequest);
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

        [HttpDelete("{stageId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteStageReply>> DeleteStage(int stageId)
        {
            try
            {
                var reply = await connections.StagesServiceClient.DeleteStageAsync(new DeleteStageRequest { StageId = stageId});
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

        [HttpPut("{stageId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UpdateStageReply>> UpdateStage([FromBody] UpdateStageRequest updateStageRequest)
        {
            try
            {
                var reply = await connections.StagesServiceClient.UpdateStageAsync(updateStageRequest);
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

        [HttpGet("board={boadId}")]
        public async Task<ActionResult<List<StageInfo>>> GetStages(string boadId)
        {
            try
            {
                var reply = await connections.StagesServiceClient.GetStagesByBoardIdAsync(new GetStagesRequest { BoardId = boadId });
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

        [HttpGet("{stageId}")]
        public async Task<ActionResult<List<BoardInfo>>> GetStage(int stageId)
        {
            try
            {
                var reply = await connections.StagesServiceClient.GetStageAsync(new GetStageRequest { StageId = stageId });
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
