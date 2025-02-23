using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;
using System.Data.Entity;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class ProjectsRepository(ProjectsDbContext context) : IProjectsRepository
    {
        public async Task<Guid> CreateAsync(ProjectModel model)
        {
            var project = new ProjectEntity
            {
                Name = model.Name,
                Description = model.Description
            };

            await context.AddAsync(project);
            await context.SaveChangesAsync();
            
            return project.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var deletingProject = context.Projects.FirstOrDefault(p => p.Id == id);
            context.Projects.Remove(deletingProject);
            await context.SaveChangesAsync();
            return deletingProject.Id;
        }

        public Task<List<ProjectModel>> GetAllAsync(Guid userId)
        {
            var res = context.Projects
                            .AsNoTracking()
                            .Join(context.Members, p => p.Id, m => m.ProjectId, (p, m) => new { m.UserId, model = EntityToModel(p) })
                            .Where(p => p.UserId == userId)
                            .Select(p => p.model)
                            .ToList();

            return Task.FromResult(res);
        }

        public Task<ProjectModel> GetByIdAsync(Guid id)
        {
            var res = context.Projects
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            return Task.FromResult(EntityToModel(res));
        }

        public async Task<Guid> UpdateAsync( ProjectModel model)
        {
            var project = context.Projects
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == model.Id);

            project.Name = project.Name;
            project.Description = project.Description;

            context.Update(project);
            await context.SaveChangesAsync();

            return project.Id;
        }


        private ProjectModel EntityToModel(ProjectEntity p)
        {
            var res = ProjectModel.Create(
                id: p.Id,
                name: p.Name,
                description: p.Description).Value;

            return res;
        }

    }
}
