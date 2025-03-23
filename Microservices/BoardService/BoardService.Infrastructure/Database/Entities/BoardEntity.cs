using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoardService.Infrastructure.Database.Entities
{
    [Table("boards")]
    public class BoardEntity
    {
        [Key]
        [Required]
        [Column("board_id")]
        public string Id { get; set; }

        [Required]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [Column("title")]
        public string Title{ get; set; }


        public BoardTypeEntity Type { get; set; }
        public List<SprintEntity> Sprints { get; set; }
        public List<TaskEntity> Tasks { get; set; }
        public List<ExecutionStageEntity> ExecutionStages { get; set; }
    }
}
