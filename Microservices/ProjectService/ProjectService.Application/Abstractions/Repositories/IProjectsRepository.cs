using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface IProjectsRepository
    {
        Task<string> CreateAsync(ProjectModel model);
        Task<string> DeleteAsync(string id);
        Task<List<ProjectModel>> GetAllAsync(string userId);
        Task<ProjectModel> GetByIdAsync(string id);
        Task<string> UpdateAsync(ProjectModel model);
    }
}
