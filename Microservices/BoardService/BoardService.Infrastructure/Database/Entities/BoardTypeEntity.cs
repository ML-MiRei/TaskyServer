using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardService.Infrastructure.Database.Entities
{
    [Table("board_types")]
    public class BoardTypeEntity
    {
        [Key]
        [Required]
        [Column("type_id")]

        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }


        public List<BoardEntity> Boards { get; set; }
    }
}
