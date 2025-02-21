using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using System.Data.Entity;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class ProjectTaskRepository(ProjectsDbContext context) : IProjectTasksRepository
    {
        public Task<int> CreateAsync(ProjectTaskModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var deletingTask = context.ProjectTasks.FirstOrDefault(p => p.Id == id);
            context.ProjectTasks.Remove(deletingTask);
            await context.SaveChangesAsync();
            return deletingTask.Id;
        }

        public async Task<List<ProjectTaskModel>> GetAllAsync(Guid parentId)
        {
            var res = context.ProjectTasks
                                       .AsNoTracking()
                                       .Where(p => p.ProjectId == parentId)
                                       .Select(p => ProjectTaskModel.Create(p.Title, p.StatusId, p.Id, p.Description).Value)
                                       .ToList();

            return res;
        }

        public async Task<ProjectTaskModel> GetByIdAsync(int id)
        {
            var res = context.ProjectTasks
                            .AsNoTracking()
                            .FirstOrDefault(p => p.Id == id);

            return ProjectTaskModel.Create(res.Title, res.StatusId, res.Id, res.Description).Value;
        }

        public async Task<int> UpdateAsync(ProjectTaskModel model)
        {
            var projectTask = context.ProjectTasks
                                       .AsNoTracking()
                                       .FirstOrDefault(p => p.Id == model.Id);

            projectTask.Title = model.Title;
            projectTask.StatusId = model.StatusId.Value;
            projectTask.Description = model.Description;
            projectTask.ExecutorId = model.Executor;
            projectTask.SprintId = model.Sprint;

            context.Update(projectTask);
            await context.SaveChangesAsync();

            return projectTask.Id;
        }
    }
}
