using Microsoft.EntityFrameworkCore;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Enums;
using TaskService.Core.Models;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Implementations.Repositories
{
    public class ExecutionsRepository(TasksDbContext context) : IExecutionsRepository
    {
        public async Task<string> CreateAsync(ExecutionModel executionModel)
        {
            var execution = new ExecutionEntity
            {
                UserId = executionModel.UserId,
                TaskId = executionModel.TaskId,
                DateStart = executionModel.DateStart,
                StatusId = (int)executionModel.Status
            };

            await context.AddAsync(execution);
            await context.SaveChangesAsync();

            return execution.TaskId;
        }

        public async Task<string> CreateAsync(List<ExecutionModel> executionModel)
        {
            await context.AddRangeAsync(executionModel.Select(e => new ExecutionEntity
            {
                UserId = e.UserId,
                TaskId = e.TaskId,
                DateStart = e.DateStart,
                StatusId = (int)e.Status
            }));
            await context.SaveChangesAsync();

            return executionModel[0].TaskId;
        }

        public async Task<List<ExecutionModel>> GetExecutorsByTaskIdAsync(string taskId)
        {
            var executions = context.Executions.AsNoTracking()
                .Where(e => e.TaskId == taskId)
                .GroupBy(e => e.UserId);

            List<ExecutionModel> result = new List<ExecutionModel>();

            foreach (var ex in executions)
            {
                var lastAction = ex.OrderByDescending(x => x.DateStart).First();
                if (lastAction.StatusId == (int)ExecutionStatus.Started)
                {
                    result.Add(new ExecutionModel(lastAction.TaskId, lastAction.UserId, (ExecutionStatus)lastAction.StatusId, lastAction.DateStart, lastAction.Id));
                }
            }

            return result;
        }


        public async Task<List<ExecutionModel>> GetHistoryExecutionsByTaskIdAsync(string taskId)
        {
            return context.Executions.AsNoTracking()
                .Where(e => e.TaskId == taskId)
                .Select(e => new ExecutionModel(e.TaskId, e.UserId, (ExecutionStatus)e.StatusId, e.DateStart, e.Id))
                .ToList();
        }

        public async Task<List<ExecutionModel>> GetStateExecutionsByUserIdAsync(string userId)
        {
            var executions = context.Executions.AsNoTracking()
                           .Where(e => e.UserId == userId)
                           .GroupBy(e => e.TaskId);

            List<ExecutionModel> result = new List<ExecutionModel>();

            foreach (var ex in executions)
            {
                var lastAction = ex.OrderByDescending(x => x.DateStart).First();
                result.Add(new ExecutionModel(lastAction.TaskId, lastAction.UserId, (ExecutionStatus)lastAction.StatusId, lastAction.DateStart, lastAction.Id));
            }

            return result;
        }


        public async Task<List<ExecutionModel>> GetHistoryExecutionsByUserIdAsync(string userId)
        {
            return context.Executions.AsNoTracking()
                .Where(e => e.UserId == userId)
                .Select(e => new ExecutionModel(e.TaskId, e.UserId, (ExecutionStatus)e.StatusId, e.DateStart, e.Id))
                .ToList();
        }
    }
}
