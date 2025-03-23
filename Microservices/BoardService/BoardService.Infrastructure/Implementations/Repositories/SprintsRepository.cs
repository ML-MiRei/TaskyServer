using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Models;
using BoardService.Infrastructure.Database;
using BoardService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Implementations.Repositories
{
    public class SprintsRepository(BoardDbContext context) : ISprintsRepository
    {
        public async Task<int> CreateAsync(SprintModel sprintModel)
        {
            var sprint = new SprintEntity
            {
                BoardId = sprintModel.BoardId,
                DateStart = sprintModel.DateStart,
                DateEnd = sprintModel.DateEnd
            };

            await context.Sprints.AddAsync(sprint);
            await context.SaveChangesAsync();

            return sprint.Id;
        }

        public async Task<int> UpdateAsync(SprintModel sprintModel)
        {
            var sprint = await context.Sprints.FindAsync(sprintModel.Id);

            sprint.DateStart = sprintModel.DateStart;
            sprint.DateEnd = sprintModel.DateEnd;

            context.Sprints.Update(sprint);
            await context.SaveChangesAsync();

            return sprint.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sprint = await context.Sprints.FindAsync(id);

            context.Sprints.Remove(sprint);
            await context.SaveChangesAsync();

            return sprint.Id;
        }

        public async Task<SprintModel> GetAsync(int id)
        {
            var stage = await context.Sprints.AsNoTracking().FirstAsync(s => s.Id == id);

            return SprintModel.Create( stage.DateStart, stage.DateEnd, stage.BoardId, stage.Id).Value;
        }

        public async Task<List<SprintModel>> GetAllAsync(string boardId)
        {
            var sprints = context.Sprints
                .AsNoTracking()
                .Where(s => s.BoardId == boardId)
                .Select(s => SprintModel.Create( s.DateStart, s.DateEnd, s.BoardId, s.Id).Value)
                .ToList();

            return sprints;
        }
    }
}
