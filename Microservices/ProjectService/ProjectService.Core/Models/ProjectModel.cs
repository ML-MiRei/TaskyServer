using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class ProjectModel
    {
        public string? Id { get; private set; }
        public string Title { get; private set; }
        public string? Details { get; private set; }

        private ProjectModel(string id, string name, string? description)
        {
            Id = id;
            Title = name;
            Details = description;
        }

        public static Result<ProjectModel> Create(string title, string? details = null, string? id = null )
        {
            var res = new ResultFactory<ProjectModel>();

            if (string.IsNullOrEmpty(title))
                res.AddError("Название не может быть пустым");

            id = string.IsNullOrEmpty(id ) ? Guid.NewGuid().ToString() : id;

            res.SetResult(new ProjectModel(id, title, details));

            return res.Create();
        }
    }
}
