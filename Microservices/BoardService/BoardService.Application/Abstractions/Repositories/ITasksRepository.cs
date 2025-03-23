using BoardService.Core.Models;

namespace BoardService.Application.Abstractions.Repositories
{
    public interface ITasksRepository
    {
        Task<string> AddAsync(TaskModel[] taskModel);
        Task<int> ChangeSprintAsync(TaskModel[] taskModels);
        Task<string> ChangeStageAsync(TaskModel taskModel);
        Task<string> DeleteAsync(string id);
        Task<List<TaskModel>> GetAllByBoardAsync(string boardId);
        Task<List<TaskModel>> GetAllBySprintAsync(int sprintId);
        Task<List<TaskModel>> GetAllByStageAsync(int stageId);
    }
}