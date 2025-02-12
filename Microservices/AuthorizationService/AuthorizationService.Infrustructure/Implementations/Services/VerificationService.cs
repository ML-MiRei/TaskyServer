using AuthorizationService.Applicaion.Abstractions.Services;
using AuthorizationService.Infrastructure.Common;
using Microsoft.AspNet.Identity;
using MimeKit;
using System;
using System.Configuration;
using System.Net.Http;

namespace AuthorizationService.Infrastructure.Implementations.Services
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
