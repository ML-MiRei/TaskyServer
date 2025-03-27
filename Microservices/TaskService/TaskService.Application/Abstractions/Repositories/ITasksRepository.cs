using TaskService.Core.Models;

namespace TaskService.Application.Abstractions.Repositories
{
    public interface ITasksRepository
    {
        Task<string> CreateAsync(TaskModel taskModel);
        Task<string> DeleteAsync(string taskId);
        Task<List<TaskModel>> GetAllByProjectId(string projectId);
        Task<List<TaskModel>> GetAllByUserId(string userId);
        Task<TaskModel> GetAsync(string taskId);
        Task<string> UpdateAsync(TaskModel taskModel);
    }
}