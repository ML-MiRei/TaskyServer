namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IVerificationService
    {
        public Task VerificateEmail(string email);
        public Task<bool> TryVerify(string email, string token);

    }
}
