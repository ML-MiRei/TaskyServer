namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IConfirmationService
    {
        public Task ConfirmationEmail(string email);
    }
}
