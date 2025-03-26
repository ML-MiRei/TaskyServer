using TaskService.Core.Common;

namespace TaskService.Core.Models
{
    public class CommentModel
    {
        public int? Id { get; }
        public string CreatorId { get; }
        public string TaskId { get; }
        public string Text { get; }
        public DateTime Created { get; }

        private CommentModel(int? id, string creatorId, string taskId, string text, DateTime created)
        {
            Id = id;
            CreatorId = creatorId;
            TaskId = taskId;
            Text = text;
            Created = created;
        }

        public static Result<CommentModel> Create(string creatorId, string taskId, string text, DateTime? created = null, int? id = null)
        {
            var resFactory = new ResultFactory<CommentModel>();

            text = text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                resFactory.AddError("Текст комментария не может отсутствовать");
                return resFactory.Create();
            }            

            created = created.HasValue ? created.Value : DateTime.UtcNow;

            resFactory.SetResult(new CommentModel(id, creatorId, taskId, text, created.Value));

            return resFactory.Create();
        }
    }
}
