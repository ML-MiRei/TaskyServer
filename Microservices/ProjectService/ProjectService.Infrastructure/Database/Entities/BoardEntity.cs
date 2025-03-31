using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    [Table("boards")]
    public class BoardEntity
    {
        [Required]
        [Column("board_id")]
        public string BoardId {  get; set; }

        [Required]
        [Column("project_id")]
        public string ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
    }
}
