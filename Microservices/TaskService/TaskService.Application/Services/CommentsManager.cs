using TaskService.Application.Abstractions.Repositories;
using TaskService.Core.Models;

namespace TaskService.Application.Services
{
    public class CommentsManager(ICommentsRepository commentsRepository)
    {
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

    }
}
