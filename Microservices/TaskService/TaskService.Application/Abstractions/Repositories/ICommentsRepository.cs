using TaskService.Core.Models;

namespace TaskService.Application.Abstractions.Repositories
{
    public interface ICommentsRepository
    {
        Task<int> CreateAsync(CommentModel commentModel);
        Task<int> DeleteAsync(CommentModel commentModel);
        Task<List<CommentModel>> GetAllByTaskAsync(string task_id);
        Task<int> UpdateAsync(CommentModel commentModel);
    }
}