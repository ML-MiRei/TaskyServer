namespace AuthorizationService.Applicaion.Abstractions.Services
{
    public interface IVerificationService
    {
        public Task VerificationEmail(string email);
    }
}
