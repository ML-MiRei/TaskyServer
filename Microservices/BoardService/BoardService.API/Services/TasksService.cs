using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Common;
using BoardService.Core.Models;
using Grpc.Core;

namespace BoardService.API.Services
{
    public class TasksService(ILogger<TasksService> logger, ITasksRepository tasksRepository) : BoardTasks.BoardTasksBase
    {
        public async override Task<AddTasksReply> AddTasks(AddTasksRequest request, ServerCallContext context)
        {
            var taskModels = request.TaskIds.Select( id => new TaskModel(id, request.BoardId, request.StageId));

            try
            {
                var reply = await tasksRepository.AddAsync(taskModels.ToArray());
                return new AddTasksReply { BoardId = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<ChangeSprintReply> ChangeSprint(ChangeSprintRequest request, ServerCallContext context)
        {
            var tasks = request.TaskId.Select(t => new TaskModel(t, request.BoardId, sprintId: request.SprintId));

            try
            {
                var reply = await tasksRepository.ChangeSprintAsync(tasks.ToArray());
                return new ChangeSprintReply { SprintId = reply }; 
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<ChangeStageReply> ChangeStage(ChangeStageRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await tasksRepository.ChangeStageAsync(new TaskModel(request.TaskId, null, request.StageId));
                return new ChangeStageReply { TaskId = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<DeleteTaskReply> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await tasksRepository.DeleteAsync(request.Id);
                return new DeleteTaskReply { Id = reply }; 
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public async override Task<GetTasksByBoardReply> GetTasksByBoard(GetTasksByBoardRequest request, ServerCallContext context)
        {
            try
            {
                var response = await tasksRepository.GetAllByBoardAsync(request.BoardId);
                if (response == null)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE + " " + request.BoardId);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                var reply = new GetTasksByBoardReply();

                reply.Tasks.AddRange(response.Select(t => new TaskInfo { TaskId = t.TaskId, SprintId = t.SprintId.Value, StageId = t.StageId.Value }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetTasksBySprintReply> GetTasksBySprint(GetTasksBySprintRequest request, ServerCallContext context)
        {
            try
            {
                var response = await tasksRepository.GetAllBySprintAsync(request.SprintId);
                if (response == null)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE + " " + request.SprintId);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                var reply = new GetTasksBySprintReply();

                reply.Tasks.AddRange(response.Select(t => new TaskInfo { TaskId = t.TaskId, SprintId = t.SprintId.Value, StageId = t.StageId.Value }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }
    }
}
