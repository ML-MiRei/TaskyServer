using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class StatusModel:BaseModel
    {
        public int? Id { get; }
        public string Name { get; }
        public Guid ProjectId { get; }

        private StatusModel(int? id, Guid projectId, string name)
        {
            Id = id;
            Name = name;
            ProjectId = projectId;
        }

        public static Result<StatusModel> Create(Guid project, string name, int? id = null)
        {
            var res = new ResultFactory<StatusModel>();

            if (string.IsNullOrEmpty(name))
            {
                res.AddError("Название не может быть пустым");
                return res.Create();
            }

            res.SetResult(new StatusModel(id, project, name));
            return res.Create();
        }
    }
}
