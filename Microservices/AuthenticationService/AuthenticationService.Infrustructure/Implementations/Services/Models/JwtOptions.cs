namespace AuthenticationService.Infrastructure.Implementations.Services.Models
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiresHours { get; set; }

    }
}
