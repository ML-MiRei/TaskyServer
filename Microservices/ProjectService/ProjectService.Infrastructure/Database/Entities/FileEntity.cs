using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class FileEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public int? ProjectTaskId { get; set; }
        public ProjectTaskEntity? ProjectTask { get; set; }
    }
}
