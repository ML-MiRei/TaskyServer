using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoardService.Infrastructure.Database.Entities
{
    [Table("tasks")]
    public class TaskEntity
    {
        [Key]
        [Required]
        [Column("task_id")]
        public string Id { get; set; }

        [Column("stage_id")]
        public int? StageId { get; set; }

        [Required]
        [Column("board_id")]
        public string BoardId { get; set; }

        [Column("sprint_id")]
        public int? SprintId { get; set; }


        public ExecutionStageEntity ExecutionStage { get; set; }
        public BoardEntity Board { get; set; }
        public SprintEntity? Sprint { get; set; }

    }
}
