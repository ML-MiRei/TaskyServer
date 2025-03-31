using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectService.Infrastructure.Database.Entities
{
    [Table("members")]
    public class MemberEntity
    {
        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("project_id")]
        public string ProjectId {  get; set; }
        public ProjectEntity Project { get; set; }

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

    }
}
