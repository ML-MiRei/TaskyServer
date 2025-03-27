using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Common;
using TaskService.Core.Models;

namespace TaskService.API.Services
{
    public class TasksService(ILogger<TasksService> logger, ITasksRepository tasksRepository) : Tasks.TasksBase
    {
        public override async Task<CreateTaskReply> CreateTask(CreateTaskRequest request, ServerCallContext context)
        {
            var taskModel = TaskModel.Create(request.Title, request.Details, request.ProjectId, parentId: request.ParentId, dateEnd: request.DateEnd.ToDateTime());

            if (taskModel.IsError)
            {
                logger.LogDebug(taskModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, taskModel.StringErrors));
            }

            try
            {
                var taskId = await tasksRepository.CreateAsync(taskModel.Value);
                Console.WriteLine("id = " + taskId);
                return new CreateTaskReply { TaskId = taskId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public override async Task<DeleteTaskReply> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
        {
            var res = await tasksRepository.DeleteAsync(request.TaskId);

            try
            {
                return new DeleteTaskReply { TaskId = res };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public override async Task<GetAllTasksByProjectReply> GetAllTasksByProject(GetAllTasksByProjectRequest request, ServerCallContext context)
        {
            try
            {
                var tasks = await tasksRepository.GetAllByProjectId(request.ProjectId);
                var reply = new GetAllTasksByProjectReply();

                reply.Tasks.AddRange(tasks.Select(t => new TaskInfo
                {
                    TaskId = t.Id,
                    DateCreated = Timestamp.FromDateTimeOffset(t.DateCreated),
                    DateEnd = t.DateEnd.HasValue ? Timestamp.FromDateTimeOffset(t.DateEnd.Value) : null,
                    Details = t.Details,
                    ParentId = t.ParentId,
                    ProjectId = t.ProjectId,
                    Title = t.Title
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetAllTasksByUserReply> GetAllTasksByUser(GetAllTasksByUserRequest request, ServerCallContext context)
        {
            try
            {
                var tasks = await tasksRepository.GetAllByUserId(request.UserId);
                var reply = new GetAllTasksByUserReply();

                reply.Tasks.AddRange(tasks.Select(t => new TaskInfo
                {
                    TaskId = t.Id,
                    DateCreated = Timestamp.FromDateTimeOffset(t.DateCreated),
                    DateEnd = t.DateEnd.HasValue ? Timestamp.FromDateTimeOffset(t.DateEnd.Value) : null,
                    Details = t.Details,
                    ParentId = t.ParentId,
                    ProjectId = t.ProjectId,
                    Title = t.Title
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetTaskReply> GetTask(GetTaskRequest request, ServerCallContext context)
        {
            try
            {
                var tasks = await tasksRepository.GetAsync(request.TaskId);

                return new GetTaskReply {
                    TaskId = tasks.Id,
                    DateCreated = Timestamp.FromDateTimeOffset(tasks.DateCreated),
                    DateEnd = tasks.DateEnd.HasValue ? Timestamp.FromDateTimeOffset(tasks.DateEnd.Value) : null,
                    Details = tasks.Details,
                    ParentId = tasks.ParentId,
                    ProjectId = tasks.ProjectId,
                    Title = tasks.Title
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public override async Task<UpdateTaskReply> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
        {
            var taskModel = TaskModel.Create(request.Title, request.Details, dateEnd: request.DateEnd.ToDateTime(), id: request.TaskId);

            if (taskModel.IsError)
            {
                logger.LogDebug(taskModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, taskModel.StringErrors));
            }

            try
            {
                var taskId = await tasksRepository.UpdateAsync(taskModel.Value);
                return new UpdateTaskReply { TaskId = taskId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }
    }
}
