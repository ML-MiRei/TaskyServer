using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities
{
    [Table("execute_history")]
    public class ExecutionEntity
    {
        [Key]
        [Required]
        [Column("execution_id")]
        public int Id { get; set; }

        [Column("task_id")]
        public string? TaskId { get; set; }
        public TaskEntity? Task { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }
        public StatusEntity Status { get; set; }

        [Required]
        [Column("date_start")]
        public DateTime DateStart { get; set; } = DateTime.UtcNow;

    }
}
