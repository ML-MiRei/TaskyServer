using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Infrastructure.Database.Entities
{
    public class AuthData
    {
        [Key]
        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsVerified { get; set; } = false;

        public AuthData(string email, string passwordHash)
        { 
            Email = email;
            PasswordHash = passwordHash;
        }


    }
}
