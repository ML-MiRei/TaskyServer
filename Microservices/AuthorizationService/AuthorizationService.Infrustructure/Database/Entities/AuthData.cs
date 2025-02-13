using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Infrastructure.Database.Entities
{
    public class AuthData
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
