using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class CommentEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedDate {  get; set; } = DateTime.Now;

        [Required]
        public Guid ProjectId {  get; set; }

        [Required]
        public int ProjectTaskId { get; set; }
        public ProjectTaskEntity ProjectTask { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
        public MemberEntity Member { get; set; }
    }
}
