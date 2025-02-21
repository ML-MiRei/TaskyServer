using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;
using System.Data.Entity;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class TaskStatusesRepository(ProjectsDbContext context) : ITaskStatusesRepository
    {
        public async Task<int> CreateAsync(StatusModel model)
        {
            var newStatus = new StatusTaskEntity
            {
                Name = model.Name,
                ProjectId = model.ProjectId
            };

            await context.AddAsync(newStatus);
            await context.SaveChangesAsync();

            return newStatus.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var status = context.Statuses
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);
            
            context.Statuses.Remove(status);
            await context.SaveChangesAsync();

            return status.Id;
        }

        public Task<List<StatusModel>> GetAllAsync(Guid parentId)
        {
            var statuses = context.Statuses
                .AsNoTracking()
                .Where(s => s.ProjectId == parentId)
                .Select(s => StatusModel.Create(s.ProjectId, s.Name, s.Id).Value)
                .ToList();

            return Task.FromResult(statuses);
        }

        public Task<StatusModel> GetByIdAsync(int id)
        {
            var res = context.Statuses
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);

            var status = StatusModel.Create(res.ProjectId, res.Name, res.Id).Value;
            return Task.FromResult(status);
        }

        public async Task<int> UpdateAsync(StatusModel model)
        {
            var status = context.Statuses
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == model.Id);

            status.Name = model.Name;

            context.Statuses.Update(status);
            await context.SaveChangesAsync();

            return status.Id;
        }
    }
}
