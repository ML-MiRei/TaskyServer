using TaskService.Core.Common;

namespace TaskService.Core.Models
{
    public class TaskModel
    {
        public string? Id { get; }
        public string Title { get; }
        public string Details { get; }
        public string? ProjectId { get; }
        public string? ParentId { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateEnd { get; }

        private TaskModel(string? id, string title, string details, string? projectId, string? parentId, DateTime dateCreated, DateTime? dateEnd)
        {
            Id = id;
            Title = title;
            Details = details;
            ProjectId = projectId;
            ParentId = parentId;
            DateCreated = dateCreated;
            DateEnd = dateEnd;
        }

        public static Result<TaskModel> Create(string title, string details, string? projectId = null, DateTime? dateCreated = null, string? parentId = null, DateTime? dateEnd = null, string? id = null)
        {
            var resFactory = new ResultFactory<TaskModel>();

            title = title.Trim();
            details = details.Trim();         
            
            if (string.IsNullOrEmpty(title))
            {
                resFactory.AddError("Заголовок не может отсутствовать");
            }

            if (string.IsNullOrEmpty(title))
            {
                resFactory.AddError("Описание не может отсутствовать");
                return resFactory.Create();
            }

            dateCreated = dateCreated.HasValue ? dateCreated.Value : DateTime.UtcNow;

            resFactory.SetResult(new TaskModel(id, title, details, projectId, parentId, dateCreated.Value, dateEnd));

            return resFactory.Create();
        }
        
    }
}
