using System.ComponentModel.DataAnnotations;
using UserService.Core.Enums;

namespace UserService.Infrastructure.Database.Entities
{
    public class User
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string? PictureKey { get; set; }

        [Required]
        public int Gender { get; set; } = (int)GenderCode.Unknown;
    }
}
