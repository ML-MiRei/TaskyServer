using TaskService.Core.Models;

namespace TaskService.Application.Abstractions.Repositories
{
    public interface ICommentsRepository
    {
        Task<int> CreateAsync(CommentModel commentModel);
        Task<int> DeleteAsync(int commentId);
        Task<List<CommentModel>> GetAllByTaskAsync(string taskId);
        Task<CommentModel> GetAsync(int commentId);
        Task<int> UpdateAsync(CommentModel commentModel);
    }
}