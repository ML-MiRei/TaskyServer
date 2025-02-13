using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Infrastructure.Common;
using Microsoft.AspNet.Identity;
using MimeKit;
using System;
using System.Configuration;
using System.Net.Http;

namespace AuthenticationService.Infrastructure.Implementations.Services
{
    class VerificationService : IVerificationService
    {


        public Task VerificationEmail(string email)
        {
            var senderEmail = ConfigurationManager.AppSettings["app_email"];


            throw new NotImplementedException();
        }
    }
}
