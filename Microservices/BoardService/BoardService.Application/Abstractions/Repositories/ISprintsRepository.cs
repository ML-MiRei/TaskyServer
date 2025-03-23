using BoardService.Core.Models;

namespace BoardService.Application.Abstractions.Repositories
{
    public interface ISprintsRepository
    {
        Task<int> CreateAsync(SprintModel sprintModel);
        Task<int> DeleteAsync(int id);
        Task<SprintModel> GetAsync(int id);
        Task<List<SprintModel>> GetAllAsync(string boardId);
        Task<int> UpdateAsync(SprintModel sprintModel);
    }
}