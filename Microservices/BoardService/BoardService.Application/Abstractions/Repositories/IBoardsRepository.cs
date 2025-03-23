using BoardService.Core.Models;

namespace BoardService.Application.Abstractions.Repositories
{
    public interface IBoardsRepository
    {
        Task<string> CreateAsync(BoardModel boardModel);
        Task<string> DeleteAsync(string id);
        Task<BoardModel> GetAsync(string id);
        Task<List<BoardModel>> GetAllAsync(string[] id);
        Task<string> UpdateAsync(BoardModel boardModel);
    }
}