using Microsoft.EntityFrameworkCore;
using TaskService.Application.Abstractions.Repositories;
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

        public async Task<ExecutionModel> GetByTaskIdAsync(string taskId)
        {
            var execution = await context.Executions.AsNoTracking().OrderByDescending(e => e.DateStart).FirstAsync(e => e.TaskId == taskId);
            return new ExecutionModel(execution.TaskId, execution.UserId, execution.StatusId, execution.DateStart, execution.Id);
        }
    }
}
