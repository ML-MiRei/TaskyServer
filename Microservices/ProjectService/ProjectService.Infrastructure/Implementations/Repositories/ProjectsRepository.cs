using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class ProjectsRepository(ProjectsDbContext context) : IProjectsRepository
    {
        public async Task<string> CreateAsync(ProjectModel model)
        {
            var project = new ProjectEntity
            {
                Title = model.Title,
                Details = model.Details,
                Id = model.Id
            };

            await context.AddAsync(project);
            await context.SaveChangesAsync();

            return project.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var deletingProject = context.Projects.FirstOrDefault(p => p.Id == id);

            context.Projects.Remove(deletingProject);
            await context.SaveChangesAsync();

            return deletingProject.Id;
        }

        public Task<List<ProjectModel>> GetAllAsync(string userId)
        {
            var res = context.Projects
                            .AsNoTracking()
                            .Where(p => p.Members.Any(m => m.UserId == userId))
                            .Select(m => ProjectModel.Create(m.Title, m.Details, m.Id).Value)
                            .ToList();

            return Task.FromResult(res);
        }

        public async Task<ProjectModel?> GetByIdAsync(string id)
        {
            var project = await context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return project == null ? null : ProjectModel.Create(project.Title, project.Details, project.Id).Value;
        }

        public async Task<string?> UpdateAsync(ProjectModel model)
        {
            var project = context.Projects.Find(model.Id);

            if (project == null)
                return null;

            project.Title = model.Title;
            project.Details = model.Details;

            await context.SaveChangesAsync();

            return project.Id;
        }

    }
}
