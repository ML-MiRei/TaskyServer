using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class BoardsRepository(ProjectsDbContext context) : IBoardsRepository
    {
        public async Task<string> CreateAsync(BoardModel boardModel)
        {
            var board = new BoardEntity { BoardId = boardModel.BoardId, ProjectId = boardModel.ProjectId };

            await context.Boards.AddAsync(board);
            await context.SaveChangesAsync();

            return board.BoardId;
        }

        public async Task<string> DeleteAsync(BoardModel boardModel)
        {
            var board = await context.Boards.FirstAsync(b => b.BoardId == boardModel.BoardId && b.ProjectId == boardModel.ProjectId);

            context.Boards.Remove(board);
            await context.SaveChangesAsync();

            return board.BoardId;
        }


        public async Task<List<BoardModel>> GetByProjectAsync(string projectId)
        {
            var boards = context.Boards.Where(b => b.ProjectId == projectId)
                .Select(b => new BoardModel(b.BoardId, b.ProjectId))
                .ToList();

            return boards;
        }
    }
}
