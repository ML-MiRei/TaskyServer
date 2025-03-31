using Microsoft.Extensions.Logging;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Abstractions.Services;
using TaskService.Application.Dtos;
using TaskService.Core.Enums;
using TaskService.Core.Models;

namespace TaskService.Application.Services
{
    public class ExecutionManager(IExecutionsRepository executionsRepository,
        ITasksRepository tasksRepository,
        ILogger<ExecutionManager> logger,
        INotificationService notificationService)
    {
        public async Task<string> AddExecutor(ExecutionModel executionModel)
        {
            executionModel.Status = ExecutionStatus.Started;
            var reply = await executionsRepository.CreateAsync(executionModel);

            Task.Run(async () =>
            {
                var task = await tasksRepository.GetAsync(executionModel.TaskId);
                var textMessage = "Вы были назначены исполнителем задачи. Удачной работы!";

                var message = new MessageModel([executionModel.UserId], textMessage, task.Title, task.Id);
                notificationService.SendNotification(message);
            });

            return reply;
        }

        public async Task<string> DeleteExecutor(ExecutionModel executionModel)
        {
            executionModel.Status = ExecutionStatus.Declined;
            var reply = await executionsRepository.CreateAsync(executionModel);

            Task.Run(async () =>
            {
                var task = await tasksRepository.GetAsync(executionModel.TaskId);
                var textMessage = "Вы были сняты с роли исполнителя задачи";

                var message = new MessageModel([executionModel.UserId], textMessage, task.Title, task.Id);
                notificationService.SendNotification(message);
            });

            return reply;
        }

        public async Task<string> SetFinishedExecutions(string taskId)
        {
            var executions = await executionsRepository.GetExecutorsByTaskIdAsync(taskId);

            for (int i = 0; i < executions.Count; i++)
            {
                executions[i].Status = ExecutionStatus.Finished;
            }

            var reply = await executionsRepository.CreateAsync(executions);

            Task.Run(async () =>
            {
                var task = await tasksRepository.GetAsync(taskId);
                var textMessage = "Работа над задачей завершена. Продолжайте в том же духе!";

                var message = new MessageModel(executions.Select(e => e.UserId).ToArray(), textMessage, task.Title, task.Id);
                notificationService.SendNotification(message);
            });

            return reply;
        }
    }
}
