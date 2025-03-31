using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectService.Infrastructure.Database.Entities
{

    [Table("roles")]
    public class RoleEntity
    {
        [Key]
        [Column("role_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public List<MemberEntity> Members { get; set; } = new List<MemberEntity>();
    }
}
