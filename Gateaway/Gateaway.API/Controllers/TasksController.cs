using Gateaway.Core.Common;
using Getaway.Core.Contracts.Executions;
using Getaway.Core.Contracts.Sprints;
using Getaway.Core.Contracts.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController(Connections connections) : ControllerBase
    {

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateTaskReply>> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            try
            {
                var reply = await connections.TasksServiceClient.CreateTaskAsync(createTaskRequest);
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

        [HttpDelete("{taskId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteTaskReply>> DeleteTask(int taskId)
        {
            try
            {
                var reply = await connections.SprintsServiceClient.DeleteSprintAsync(new DeleteSprintRequest { Id = taskId });
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

        [HttpPut("{taskId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UpdateTaskReply>> UpdateSprint([FromBody] UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                var reply = await connections.TasksServiceClient.UpdateTaskAsync(updateTaskRequest);
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

        [HttpGet("project={projectId}")]
        public async Task<ActionResult<GetAllTasksByProjectRequest>> GetTasksByProjectId(string projectId)
        {
            try
            {
                var reply = await connections.TasksServiceClient.GetAllTasksByProjectAsync(new GetAllTasksByProjectRequest { ProjectId = projectId });
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

        [HttpGet("user={userId}")]
        public async Task<ActionResult<GetAllTasksByUserRequest>> GetTasksByUserId(string userId)
        {
            try
            {
                var reply = await connections.TasksServiceClient.GetAllTasksByUserAsync(new GetAllTasksByUserRequest { UserId = userId });
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

        [HttpGet("{taskId}")]
        public async Task<ActionResult<GetTaskReply>> GetTask(string taskId)
        {
            try
            {
                var reply = await connections.TasksServiceClient.GetTaskAsync(new GetTaskRequest { TaskId = taskId });
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


        [HttpPost("{taskId}/executors")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AddExecutorReply>> AddExecutor([FromBody] AddExecutorRequest addExecutorRequest)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.AddExecutorAsync(addExecutorRequest);
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

        [HttpDelete("{taskId}/executors/{executorId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteExecutorReply>> DeleteExecutor(string taskId, string executorId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.DeleteExecutorAsync(new DeleteExecutorRequest { ExecutorId = executorId, TaskId = taskId });
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

        [HttpGet("{taskId}/executors")]
        public async Task<ActionResult<GetExecutorsReply>> GetExecutors(string taskId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.GetExecutorsAsync(new GetExecutorsRequest { TaskId = taskId});
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

        [HttpGet("{taskId}/executors/history")]
        public async Task<ActionResult<GetHistoryByTaskReply>> GetExecutorsHistory(string taskId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.GetHistoryByTaskAsync(new GetHistoryByTaskRequest { TaskId = taskId });
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

        [HttpGet("executions/history/{userId}")]
        public async Task<ActionResult<GetHistoryByUserReply>> GetExecutionsHistoryByUserId(string userId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.GetHistoryByUserAsync(new GetHistoryByUserRequest { ExecutorId = userId });
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

        [HttpGet("executions/state/{userId}")]
        public async Task<ActionResult<GetStateExecutionsByUserReply>> GetExecutionsStateByUserId(string userId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.GetStateExecutionsByUserAsync(new GetStateExecutionsByUserRequest { ExecutorId = userId });
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

        [HttpPost("{taskId}/set-finish")]
        public async Task<ActionResult<SetFinishedExecutionsReply>> SetFinishExecution(string taskId)
        {
            try
            {
                var reply = await connections.ExecutionsServiceClient.SetFinishedExecutionsAsync(new SetFinishedExecutionsRequest
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
