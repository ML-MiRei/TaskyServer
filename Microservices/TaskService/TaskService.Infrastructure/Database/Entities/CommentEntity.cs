using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskService.Infrastructure.Database.Entities
{
    [Table("comments")]
    public class CommentEntity
    {
        [Key]
        [Required]
        [Column("comment_id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("task_id")]
        public string TaskId { get; set; }
        public TaskEntity Task { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }

        [Required]
        [Column("date_created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
