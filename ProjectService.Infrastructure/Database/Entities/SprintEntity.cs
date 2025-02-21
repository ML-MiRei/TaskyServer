using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class SprintEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }

        [Required]
        public DateTime DateStart {  get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public List<ProjectTaskEntity> ProjectTasks { get; set; } = new List<ProjectTaskEntity>();
    }
}
