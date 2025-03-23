using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Models;
using BoardService.Infrastructure.Database;
using BoardService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Implementations.Repositories
{
    public class BoardsRepository(BoardDbContext context) : IBoardsRepository
    {
        public async Task<string> CreateAsync(BoardModel boardModel)
        {
            var board = new BoardEntity
            {
                Id = boardModel.Id,
                Title = boardModel.Title,
                TypeId = (int)boardModel.Type,
            };

            await context.Boards.AddAsync(board);
            await context.SaveChangesAsync();

            return board.Id;
        }

        public async Task<string> UpdateAsync(BoardModel boardModel)
        {
            var board = await context.Boards.FirstAsync(b => b.Id == boardModel.Id);

            board.Title = boardModel.Title;

            //context.Boards.Update(board);
            await context.SaveChangesAsync();

            return board.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var board = await context.Boards.FindAsync(id);

            context.Boards.Remove(board);
            await context.SaveChangesAsync();

            return board.Id;
        }

        public async Task<BoardModel> GetAsync(string id)
        {
            var board = await context.Boards.AsNoTracking().FirstAsync(s => s.Id == id);

            return BoardModel.Create(board.Title, (Core.Enums.BoardType)board.TypeId, board.Id).Value;
        }

        public async Task<List<BoardModel>> GetAllAsync(string[] ids)
        {
            var boards = await context.Boards
                .AsNoTracking()
                .Where(s => ids.Contains(s.Id))
                .Select(b => BoardModel.Create(b.Title, (Core.Enums.BoardType)b.TypeId, b.Id).Value)
                .ToListAsync();

            return boards;
        }
    }
}
