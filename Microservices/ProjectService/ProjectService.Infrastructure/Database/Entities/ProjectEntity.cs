using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    [Table("projects")]
    public class ProjectEntity
    {
        [Key]
        [Column("project_id")]
        public string Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("details")]
        public string? Details { get; set; }


        public List<BoardEntity> Sprints { get; set; } = new List<BoardEntity>();
        public List<MemberEntity> Members { get; set; } = new List<MemberEntity> ();

    }
}
