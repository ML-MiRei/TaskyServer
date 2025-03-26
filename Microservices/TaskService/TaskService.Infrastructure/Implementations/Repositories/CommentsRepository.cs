using Microsoft.EntityFrameworkCore;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Models;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Implementations.Repositories
{
    public class CommentsRepository(TasksDbContext context) : ICommentsRepository
    {
        public async Task<int> CreateAsync(CommentModel commentModel)
        {
            var comment = new CommentEntity { DateCreated = commentModel.Created, TaskId = commentModel.TaskId, Text = commentModel.Text, UserId = commentModel.CreatorId };
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> UpdateAsync(CommentModel commentModel)
        {
            var comment = await context.Comments.FindAsync(commentModel.Id);
            comment.Text = commentModel.Text;

            await context.SaveChangesAsync();

            return comment.Id;
        }
        public async Task<int> DeleteAsync(CommentModel commentModel)
        {
            var comment = await context.Comments.FindAsync(commentModel.Id);

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return comment.Id;
        }

        public Task<List<CommentModel>> GetAllByTaskAsync(string task_id)
        {
            var comments = context.Comments.AsNoTracking()
                .Where(c => c.TaskId == task_id)
                .Select(c => CommentModel.Create(c.UserId, c.TaskId, c.Text, c.DateCreated, c.Id).Value)
                .ToList();

            return Task.FromResult(comments);
        }

    }
}
