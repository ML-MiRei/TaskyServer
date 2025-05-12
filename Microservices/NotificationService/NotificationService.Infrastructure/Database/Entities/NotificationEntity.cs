using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Infrastructure.Database.Entities
{
    [Table("notifications")]
    public class NotificationEntity
    {
        [Key]
        [Column("notification_id")]
        public int Id { get; set; }

        [Column("title")]
        [Required]
        public string Title { get; set; }

        [Column("text")]
        [Required]
        public string Text { get; set; }

        [Column("date")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [Column("object_id")]
        [Required]
        public string ObjectId {get; set; }

        [Column("object_type_id")]
        [Required]
        public int ObjectTypeId {  get; set; }
        public ObjectTypeEntity ObjectType { get; set; }
    }
}
