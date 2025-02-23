using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using System.Threading.Tasks;
using TaskService.API;

namespace ProjectService.API.Services
{
    public class ProjectTasksService(IProjectTasksRepository projectTasksRepository, ILogger<ProjectTasksService> logger) : Tasks.TasksBase
    {

        // add info users in getter


        public override async Task<CreateTaskReply> CreateTask(CreateTaskRequest request, ServerCallContext context)
        {
            //change logic files
            var taskRes = ProjectTaskModel.Create(
                title: request.Title,
                status: request.StatusId,
                description: request.Description,
                project: Guid.Parse(request.ProjectId),
                sprint: request.SprintId,
                executor: Guid.Parse(request.ExecutorId)
               );

            if (!taskRes.IsSuccess)
            {
                logger.LogDebug($"Task is not created, errors: {string.Join(',', taskRes.Errors)}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', taskRes.Errors)));
            }

            try
            {
                var newTask = taskRes.Value;
                var taskId = await projectTasksRepository.CreateAsync(newTask);

                return new CreateTaskReply { TaskId = taskId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));
            }

        }

        public override async Task<DeleteTaskReply> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
        {
            try
            {
                var deletedTaskId = await projectTasksRepository.DeleteAsync(request.TaskId);
                return new DeleteTaskReply { TaskId = deletedTaskId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка удаления. Повторите попытку позже"));
            }
        }

        public override async Task<GetAllTasksReply> GetAllTasks(GetAllTasksRequest request, ServerCallContext context)
        {
            try
            {
                var tasks = await projectTasksRepository.GetAllAsync(Guid.Parse(request.ProjectId));
                var res = new GetAllTasksReply();

                res.Tasks.AddRange(tasks.Select(t => new TaskShortInfo { Description = t.Description, Id = t.Id.Value, SprintId = t.Sprint, StatusId = t.StatusId.Value, Title = t.Title }));
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));

            }
        }

        public override async Task<GetTaskReply> GetTask(GetTaskRequest request, ServerCallContext context)
        {
            try
            {
                var task = await projectTasksRepository.GetByIdAsync(request.TaskId);
                return new GetTaskReply { Description = task.Description, Title = task.Title, Id = task.Id.Value, StatusId = task.StatusId.Value, Executor = new ExecutorInfo { UserId = task.Executor.ToString() } };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));

            }
        }

        public override async Task<UpdateTaskReply> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
        {
            var taskRes = ProjectTaskModel.Create(
                id: request.Id,
                title: request.Title,
                status: request.StatusId,
                description: request.Description,
                sprint: request.SprintId,
                executor: Guid.Parse(request.Executor)
               );

            if (!taskRes.IsSuccess)
            {
                logger.LogDebug($"Task {request.Id} is not updated, errors: {string.Join(',', taskRes.Errors)}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', taskRes.Errors)));
            }

            try
            {
                var newTask = taskRes.Value;
                var taskId = await projectTasksRepository.UpdateAsync(newTask);

                return new UpdateTaskReply { TaskId = taskId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));

            }
        }
    }
}
