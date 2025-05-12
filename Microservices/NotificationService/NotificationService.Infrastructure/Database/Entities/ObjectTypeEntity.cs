using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Infrastructure.Database.Entities
{
    [Table("object_types")]
    public class ObjectTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("type_id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }


        public List<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
    }
}
