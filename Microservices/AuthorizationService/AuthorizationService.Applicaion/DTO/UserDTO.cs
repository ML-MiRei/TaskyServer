using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
