namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IVerificationService
    {
        public Task VerificateEmail(string email);
    }
}
