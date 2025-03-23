using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardService.Infrastructure.Database.Entities
{
    [Table("execution_stages")]
    public class ExecutionStageEntity
    {
        [Key]
        [Required]
        [Column("stage_id")]

        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("board_id")]
        public string BoardId { get; set; }

        [Column("max_tasks_count")]
        public int? MaxTasksCount { get; set; }

        [Column("queue")]
        public int Queue { get; set; }


        public BoardEntity Board { get; set; }
        public List<TaskEntity> Tasks { get; set; }
    }
}
