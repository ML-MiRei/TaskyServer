namespace EmailService.Models
{
    public class SmtpOptions
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name {  get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        
    }
}
