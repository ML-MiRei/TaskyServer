using Gateaway.Core.Common;
using Gateaway.Core.ReplyModels;
using Gateaway.Core.RequestModels.Boards;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.BoardTasks;
using Getaway.Core.Enums;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BoardsController(Connections connections) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<BoardModel>>> GetListBoards([FromBody] GetListBoardsRequest getBoardsRequest)
        {
            try
            {
                var response = new List<BoardModel>();

                var request = new GetBoardsRequest();
                request.Id.AddRange(getBoardsRequest.BoardsIds);
                var reply = await connections.BoardsServiceClient.GetBoardsAsync(request);

                List<Task> tasks = new List<Task>();

                foreach (var board in reply.Boards)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var stages = (await connections.StagesServiceClient.GetStagesByBoardIdAsync(new Core.Contracts.Stages.GetStagesRequest
                        {
                            BoardId = board.Id
                        })).Stages.Select(s => new StageModel
                        {
                            Id = s.StageId,
                            Name = s.Name,
                            Queue = s.Queue,
                            MaxTasksCount = s.MaxTasksCount,
                        }).ToArray();

                        var tasks = (await connections.BoardTasksServiceClient.GetTasksByBoardAsync(new GetTasksByBoardRequest { BoardId = board.Id }))
                        .Tasks.Select(t => new BoardTaskModel
                        {
                            TaskId = t.TaskId,
                            StageId = t.StageId,
                            SprintId = t.SprintId,
                        }).ToArray();

                        SprintModel[] sprints = null;

                        if (board.Type == (int)BoardType.SCRUM)
                        {
                            sprints = (await connections.SprintsServiceClient.GetSprintsAsync(new Core.Contracts.Sprints.GetSprintsRequest { BoardId = board.Id }))
                            .Sprints.Select(s => new SprintModel
                            {
                                DateStart = s.DateStart.ToDateTime(),
                                DateEnd = s.DateEnd.ToDateTime(),
                                Id = s.Id,
                            }).ToArray();
                        }

                        response.Add(new BoardModel
                        {
                            Id = board.Id,
                            Title = board.Title,
                            Type = (BoardType)board.Type,
                            Sprints = sprints,
                            Stages = stages,
                            Tasks = tasks
                        });

                    }));
                }

                Task.WaitAll(tasks.ToArray());

                return Ok(response);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = 404, Content = ex.Message };
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> CreateBoard([FromBody] Gateaway.Core.RequestModels.Boards.CreateBoardRequest createBoardRequest)
        {
            try
            {
                var reply = await connections.BoardsServiceClient.CreateBoardAsync(new Core.Contracts.Boards.CreateBoardRequest
                {
                    Title = createBoardRequest.Title,
                    Type = createBoardRequest.Type
                });
                return Ok(reply.Id);
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.ToString());
                return new ContentResult() { StatusCode = 404, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{boardId}")]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteBoardReply>> DeleteBoard(string boardId)
        {
            try
            {
                var reply = await connections.BoardsServiceClient.DeleteBoardAsync(new DeleteBoardRequest { Id = boardId });
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = 404, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{boardId}")]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<UpdateBoardReply>> UpdateBoard(string boardId, [FromBody] Gateaway.Core.RequestModels.Boards.UpdateBoardRequest updateBoardRequest)
        {
            try
            {
                var reply = await connections.BoardsServiceClient.UpdateBoardAsync(new Core.Contracts.Boards.UpdateBoardRequest { Id = boardId, Title = updateBoardRequest.Title});
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = 404, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        [HttpPost("{boadId}/tasks")]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<AddTasksReply>> AddTasks(string boardId, [FromBody] Gateaway.Core.RequestModels.Boards.AddTasksRequest addTasksRequest)
        {
            try
            {
                var request = new Core.Contracts.BoardTasks.AddTasksRequest
                {
                    BoardId = boardId,
                    StageId = addTasksRequest.StageId
                };

                request.TaskIds.AddRange(addTasksRequest.TasksIds);

                var reply = await connections.BoardTasksServiceClient.AddTasksAsync(request);
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

        [HttpDelete("{boadId}/tasks/{taskId}")]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<DeleteTaskReply>> DeleteTask(string taskId)
        {
            try
            {
                var reply = await connections.BoardTasksServiceClient.DeleteTaskAsync(new DeleteTaskRequest { Id = taskId });
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

        [HttpPut("{boadId}/tasks/{taskId}")]
        public async Task<ActionResult<ChangeStageReply>> ChangeStage(string taskId, [FromQuery] int stageId)
        {
            try
            {
                var reply = await connections.BoardTasksServiceClient.ChangeStageAsync(new ChangeStageRequest
                {
                    TaskId = taskId,
                    StageId = stageId
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

        //delete
        [HttpGet("{boadId}/tasks")]
        public async Task<ActionResult<GetTasksByBoardReply>> GetTasksByBoard(string boadId)
        {
            try
            {
                var reply = await connections.BoardTasksServiceClient.GetTasksByBoardAsync(new GetTasksByBoardRequest { BoardId = boadId });
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

        //delete
        [HttpGet("{boadId}/{sprintId}/tasks")]
        public async Task<ActionResult<List<BoardInfo>>> GetTasksBySprint(int sprintId)
        {
            try
            {
                var reply = await connections.BoardTasksServiceClient.GetTasksBySprintAsync(new GetTasksBySprintRequest { SprintId = sprintId });
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
