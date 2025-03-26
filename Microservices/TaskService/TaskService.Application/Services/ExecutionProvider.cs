using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Enums;
using TaskService.Core.Models;

namespace TaskService.Application.Services
{
    public class ExecutionProvider(IExecutionsRepository repository, ILogger<ExecutionProvider> logger)
    {
        public async Task<string> AddExecutor(ExecutionModel executionModel)
        {
            var execution = await repository.GetByTaskIdAsync(executionModel.TaskId);
            if (execution != null)
            {
                execution.Status = ExecutionStatus.Declined;
                await repository.CreateAsync(execution);
                logger.LogInformation($"Task {execution.Id} change status on 'Declined'");
            }

            var reply = await repository.CreateAsync(executionModel);

            return reply;
        }

        public async Task<string> DeleteExecutor(string taskId)
        {
            var execution = await repository.GetByTaskIdAsync(taskId);

            execution.Status = ExecutionStatus.Declined;
            await repository.CreateAsync(execution);

            var reply = await repository.CreateAsync(execution);

            return reply;
        }

        public async Task<string> SetEndExecution(string taskId)
        {
            var execution = await repository.GetByTaskIdAsync(taskId);

            execution.Status = ExecutionStatus.Finished;
            await repository.CreateAsync(execution);

            var reply = await repository.CreateAsync(execution);

            return reply;
        }


    }
}
