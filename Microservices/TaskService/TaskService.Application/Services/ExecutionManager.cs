using Microsoft.Extensions.Logging;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Enums;
using TaskService.Core.Models;

namespace TaskService.Application.Services
{
    public class ExecutionManager(IExecutionsRepository repository, ILogger<ExecutionManager> logger)
    {
        public async Task<string> AddExecutor(ExecutionModel executionModel)
        {
            executionModel.Status = ExecutionStatus.Started;
            var reply = await repository.CreateAsync(executionModel);
            return reply;
        }

        public async Task<string> DeleteExecutor(ExecutionModel executionModel)
        {
            executionModel.Status = ExecutionStatus.Declined;
            var reply = await repository.CreateAsync(executionModel);

            return reply;
        }

        public async Task<string> SetFinishedExecutions(string taskId)
        {
            var executions = await repository.GetExecutorsByTaskIdAsync(taskId);

            for (int i = 0; i < executions.Count; i++)
            {
                executions[i].Status = ExecutionStatus.Finished;
            }

            var reply = await repository.CreateAsync(executions);

            return reply;
        }
    }
}
