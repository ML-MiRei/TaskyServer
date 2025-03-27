using Microsoft.EntityFrameworkCore;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Models;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Implementations.Repositories
{
    public class TasksRepository(TasksDbContext context) : ITasksRepository
    {
        public async Task<string> CreateAsync(TaskModel taskModel)
        {
            var task = new TaskEntity
            {
                Title = taskModel.Title,
                Details = taskModel.Details,
                ProjectId = taskModel.ProjectId,
                DateCreated = taskModel.DateCreated,
                DateEnd = taskModel.DateEnd,
                ParentId = taskModel.ParentId
            };

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            return taskModel.Id;
        }

        public async Task<string> UpdateAsync(TaskModel taskModel)
        {
            var task = await context.Tasks.FindAsync(taskModel.Id);

            task.Title = taskModel.Title;
            task.Details = taskModel.Details;
            task.DateEnd = taskModel.DateEnd;

            await context.SaveChangesAsync();

            return taskModel.Id;
        }

        public async Task<string> DeleteAsync(string taskId)
        {
            var task = await context.Tasks.FindAsync(taskId);

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return task.Id;
        }

        public Task<List<TaskModel>> GetAllByProjectId(string projectId)
        {
            var tasks = context.Tasks.AsNoTracking()
                .Where(task => task.ProjectId == projectId)
                .Select(t => TaskModel.Create(t.Title, t.Details, t.ProjectId, t.DateCreated, t.ParentId, t.DateEnd, t.Id).Value)
                .ToList();

            return Task.FromResult(tasks);
        }

        public Task<List<TaskModel>> GetAllByUserId(string userId)
        {
            var tasks = context.Tasks.AsNoTracking()
                .Where(task => task.Executions.Any(e => e.UserId == userId))
                .Select(t => TaskModel.Create(t.Title, t.Details, t.ProjectId, t.DateCreated, t.ParentId, t.DateEnd, t.Id).Value)
                .ToList();

            return Task.FromResult(tasks);
        }
    }
}
