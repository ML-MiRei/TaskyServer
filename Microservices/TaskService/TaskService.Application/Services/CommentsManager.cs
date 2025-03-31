using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Abstractions.Services;
using TaskService.Application.Dtos;
using TaskService.Core.Models;

namespace TaskService.Application.Services
{
    public class CommentsManager(ICommentsRepository commentsRepository, INotificationService notificationService, IExecutionsRepository executionsRepository, ITasksRepository tasksRepository)
    {
        private const string COMMENT_CREATED_MESSAGE = "К задаче был добавлен новый комментарий";

        public async Task<bool> UpdateComment(CommentModel commentModel)
        {
            var comment = await commentsRepository.GetAsync(commentModel.Id.Value);

            if (comment.CreatorId != commentModel.CreatorId || (DateTime.UtcNow - comment.Created).Minutes > 3)
                return false;

            await commentsRepository.UpdateAsync(commentModel);
            return true;
        }


        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await commentsRepository.GetAsync(commentId);

            if ((DateTime.UtcNow - comment.Created).Minutes > 3)
                return false;

            await commentsRepository.DeleteAsync(commentId);
            return true;
        }


        public async Task<int> CreateComment(CommentModel comment)
        {
            var commentId = await commentsRepository.CreateAsync(comment);

            Task.Run(async () =>
            {
                var users = await executionsRepository.GetExecutorsByTaskIdAsync(comment.TaskId);
                var task = await tasksRepository.GetAsync(comment.TaskId);

                var message = new MessageModel(users.Select(u => u.UserId).ToArray(), COMMENT_CREATED_MESSAGE, task.Title, task.Id);

                notificationService.SendNotification(message);
            });

            return commentId;
        }

    }
}
