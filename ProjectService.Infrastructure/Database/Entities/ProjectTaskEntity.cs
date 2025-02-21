using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class ProjectTaskEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }

        public Guid? ExecutorId{ get; set; }
        public MemberEntity? Member { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        public ProjectEntity? Project {  get; set; }

        public int? SprintId {  get; set; }
        public SprintEntity? Sprint { get; set; }

        public int StatusId { get; set; }
        public StatusTaskEntity Status { get; set; }


        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public List<FileEntity> Files { get; set; } = new List<FileEntity>();
    }
}
