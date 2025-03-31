using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface IBoardsRepository
    {
        Task<string> CreateAsync(BoardModel boardModel);
        Task<string> DeleteAsync(BoardModel boardModel);
        Task<List<BoardModel>> GetByProjectAsync(string projectId);
    }
}