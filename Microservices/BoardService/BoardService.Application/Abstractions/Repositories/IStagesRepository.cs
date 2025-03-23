using BoardService.Core.Models;

namespace BoardService.Application.Abstractions.Repositories
{
    public interface IStagesRepository
    {
        Task<int> CreateAsync(StageModel stageModel);
        Task CreateAsync(StageModel[] stageModels);
        Task<int> DeleteAsync(int id);
        Task<StageModel> GetAsync(int id);
        Task<List<StageModel>> GetAllAsync(string boardId);
        Task<int> UpdateAsync(StageModel stageModel);
        Task<int?> GetPrevStageIdAsync(int id);

    }
}