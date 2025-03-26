using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities
{
    [Table("execute_statuses")]
    public class StatusEntity
    {
        [Key]
        [Required]
        [Column("status_id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        public List<ExecutionEntity> Executions { get; set; } = new List<ExecutionEntity>();
    }
}
