using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities
{
    [Table("tasks")]
    public class TaskEntity
    {
        [Key]
        [Required]
        [Column("task_id")]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("details")]
        public string Details { get; set; }

        [Required]
        [Column("project_id")]
        public string ProjectId { get; set; }

        [Column("parent_id")]
        public string? ParentId { get; set; }
        public TaskEntity Parent { get; set; }

        [Required]
        [Column("date_created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Column("date_end")]
        public DateTime? DateEnd { get; set; }


        public List<ExecutionEntity> Executions { get; set; } = new List<ExecutionEntity>();
        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
    }
}
