using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class MemberEntity
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProjectId {  get; set; }
        public ProjectEntity Project { get; set; }

        [Required]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public List<ProjectTaskEntity> ProjectTasks { get; set; } = new List<ProjectTaskEntity>();

    }
}
