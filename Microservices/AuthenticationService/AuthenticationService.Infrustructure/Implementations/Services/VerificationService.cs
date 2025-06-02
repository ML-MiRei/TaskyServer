using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Infrastructure.Implementations.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Infrastructure.Implementations.Services
{
    public class VerificationService(IVerificationTokenProvider verificationTokenProvider, IAuthDataRepository authDataRepository, ILogger<VerificationService> logger, IVerificationEmailSender emailSender, IOptions<VerificationOptions> options) : IVerificationService
    {

        public async Task VerificateEmail(string email)
        {
            var userId = authDataRepository.GetByEmail(email).Result.UserId.ToString();
            var varificationToken = await verificationTokenProvider.GenerateTokenAsync(userId);
            await emailSender.SendEmail(email, GetVerificationLink(varificationToken, userId));
        }

        public async Task<bool> TryVerify(string userId, string token)
        {
            var res = await verificationTokenProvider.ValidateTokenAsync(userId, token);
            if (res)
            {
                await authDataRepository.SetIsVerify(userId);
            }

            return res;
        }

        private string GetVerificationLink(string varificationToken, string userId) => $"{options.Value.ConfirmationLink}?userId={userId}&token={varificationToken}";
    }

}
