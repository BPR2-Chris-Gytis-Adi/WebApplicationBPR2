using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplicationBPR2.ViewModels;

namespace WebApplicationBPR2.Services
{
    public class MailService : IMailService
    {
        private readonly EmailConfig _emailConfig;

        public MailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public void SendMail(string email, string subject, string mess)
        {
            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = mess };

            using (var client = new SmtpClient())
            {
                client.LocalDomain = _emailConfig.LocalDomain;

                client.Connect(_emailConfig.MailServerAddress, Convert.ToInt32(_emailConfig.MailServerPort), SecureSocketOptions.Auto);
                client.Authenticate(new NetworkCredential(_emailConfig.UserId, _emailConfig.UserPassword));
                
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
