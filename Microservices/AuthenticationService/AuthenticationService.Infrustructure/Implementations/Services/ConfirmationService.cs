using AuthenticationService.Applicaion.Abstractions.Services;
using System.Configuration;

namespace AuthenticationService.Infrastructure.Implementations.Services
{
    class ConfirmationService : IConfirmationService
    {

        public Task ConfirmationEmail(string email)
        {
            var senderEmail = ConfigurationManager.AppSettings["app_email"];


            throw new NotImplementedException();
        }
    }
}
