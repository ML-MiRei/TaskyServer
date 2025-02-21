using System.ComponentModel.DataAnnotations;

namespace ProjectService.Infrastructure.Database.Entities
{
    public class RoleEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<MemberEntity> Members { get; set; } = new List<MemberEntity>();
    }
}
