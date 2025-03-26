using TaskService.Core.Models;

namespace TaskService.Application.Abstractions.Repositories
{
    public interface IExecutionsRepository
    {
        Task<string> CreateAsync(ExecutionModel executionModel);
        Task<ExecutionModel> GetByTaskIdAsync(string taskId);
    }
}
