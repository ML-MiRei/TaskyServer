
namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IVerificationEmailSender
    {
        Task SendEmail(string email, string verificationLink);
    }
}