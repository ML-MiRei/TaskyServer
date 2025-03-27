using TaskService.Core.Models;

namespace TaskService.Application.Abstractions.Repositories
{
    public interface IExecutionsRepository
    {
        Task<string> CreateAsync(ExecutionModel executionModel);
        Task<string> CreateAsync(List<ExecutionModel> executionModel);
        Task<List<ExecutionModel>> GetExecutorsByTaskIdAsync(string taskId);
        Task<List<ExecutionModel>> GetHistoryExecutionsByTaskIdAsync(string taskId);
        Task<List<ExecutionModel>> GetHistoryExecutionsByUserIdAsync(string userId);
        Task<List<ExecutionModel>> GetStateExecutionsByUserIdAsync(string userId);
    }
}
