namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IVerificationTokenProvider
    {
        public Task<string> GenerateTokenAsync(string userId);
        public Task<bool> ValidateTokenAsync(string userId, string token);


    }
}
