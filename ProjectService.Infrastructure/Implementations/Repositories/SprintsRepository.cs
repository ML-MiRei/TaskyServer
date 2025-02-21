using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;
using System.Data.Entity;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class SprintsRepository(ProjectsDbContext context) : ISprintsRepository
    {
        public async Task<int> CreateAsync(SprintModel model)
        {
            var newSprint = new SprintEntity
            {
                DateEnd = model.DateEnd,
                DateStart = model.DateStart,
                ProjectId = model.ProjectId
            };

            await context.AddAsync(newSprint);
            await context.SaveChangesAsync();

            return newSprint.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sprint = context.Sprints
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);

            context.Sprints.Remove(sprint);
            await context.SaveChangesAsync();

            return id;
        }

        public Task<List<SprintModel>> GetAllAsync(Guid parentId)
        {
            var sprints = context.Sprints
                .AsNoTracking()
                .Where(s => s.ProjectId == parentId)
                .Select(s => SprintModel.Create(s.ProjectId, s.DateStart, s.DateEnd, s.Id).Value)
                .ToList();

            return Task.FromResult(sprints);
        }

        public Task<SprintModel> GetByIdAsync(int id)
        {
            var res = context.Sprints
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);

            var sprint = SprintModel.Create(res.ProjectId, res.DateStart, res.DateEnd, res.Id).Value;
            return Task.FromResult(sprint);
        }

        public async Task<int> UpdateAsync(SprintModel model)
        {
            var sprint = context.Sprints
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == model.Id);

            sprint.DateStart = model.DateStart;
            sprint.DateEnd = model.DateEnd;

            context.Sprints.Update(sprint);
            await context.SaveChangesAsync();

            return sprint.Id;
        }
    }
}
