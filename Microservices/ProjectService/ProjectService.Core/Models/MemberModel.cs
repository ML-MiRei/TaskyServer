namespace ProjectService.Core.Models
{
    public class MemberModel
    {
        public string Id { get; private set; }
        public string ProjectId { get; private set; }
        public RoleModel Role {  get; set; }

        public MemberModel(string id, string projectId, RoleModel? role = null)
        {
            Id = id;
            ProjectId = projectId;
            Role = role;
        }
    }
}
