using Grpc.Core;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailService.Services
{
    public class SenderService(ILogger<SenderService> logger) : Sender.SenderBase
    {

        public override Task<SendMailReply> SendMail(SendMailRequest request, ServerCallContext context)
        {
            ConfigurationManager manager = new();
            var settings = manager.GetSection("SmtpSettings");

            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(settings.GetSection("Name").Value, settings.GetSection("Email").Value));

                foreach(var recipient in request.To)
                    message.To.Add(new MailboxAddress("", recipient));

                message.Subject = request.Subject;
                message.Body = new BodyBuilder() { HtmlBody = request.Body }.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(settings.GetSection("Host").Value, int.Parse(settings.GetSection("Port").Value), true);
                    client.Authenticate(settings.GetSection("Email").Value, settings.GetSection("Password").Value);
                    var s = client.Send(message);

                    client.Disconnect(true);
                    logger.LogInformation($"The message (subject: \"{request.Subject}\") to {request.To} was sent successfully");
                }
                return Task.FromResult(new SendMailReply
                {
                    IsSuccess = true,
                    Error = null
                });
            }
            catch (SmtpCommandException e)
            {
                if (e.ErrorCode == SmtpErrorCode.RecipientNotAccepted)
                {
                    logger.LogError($"The message (subject: \"{request.Subject}\") to {request.To} was not sent. Reason: \'Recipient not accepted\'");
                    return Task.FromResult(new SendMailReply
                    {
                        Error = "Неверный адрес электронной почты",
                        IsSuccess = false
                    });
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.GetBaseException().Message);
            }

            return Task.FromResult(new SendMailReply
            {
                IsSuccess = false,
                Error = "Ошибка на стороне сервера. Повторите попытку позже"
            });

        }
    }
}
