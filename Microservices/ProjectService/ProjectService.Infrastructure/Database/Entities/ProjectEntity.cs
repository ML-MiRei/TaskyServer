using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class ProjectEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }


        public List<ProjectTaskEntity> ProjectTasks { get; set; } = new List<ProjectTaskEntity>();
        public List<SprintEntity> Sprints { get; set; } = new List<SprintEntity>();
        public List<MemberEntity> Members { get; set; } = new List<MemberEntity> ();
        public List<FileEntity> Files { get; set; } = new List<FileEntity> ();

    }
}
