using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardService.Infrastructure.Database.Entities
{
    [Table("sprints")]
    public class SprintEntity
    {
        [Key]
        [Required]
        [Column("sprint_id")]
        public int Id { get; set; }

        [Required]
        [Column("board_id")]
        public string BoardId { get; set; }

        [Required]
        [Column("date_start")]
        public DateTime DateStart { get; set; }

        [Required]
        [Column("date_end")]
        public DateTime DateEnd { get; set; }


        public BoardEntity Board { get; set; }
        public List<TaskEntity> Tasks { get; set; }
    }
}
