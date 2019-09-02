using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.Email

{
    public class EmailService
    {
        private readonly EmailServiceOptions _emailServiceOptions;

        public EmailService(EmailServiceOptions emailServiceOptions)
        {
            _emailServiceOptions = emailServiceOptions;
        }

        public (bool Success, string Message) Send()
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _emailServiceOptions.FromName,
                    _emailServiceOptions.FromEmail));
                message.To.Add(new MailboxAddress(_emailServiceOptions.To));
                message.Subject = _emailServiceOptions.Subject;

                message.Body = new TextPart(_emailServiceOptions.TextFormat)
                {                   
                    Text =$"From email: <b>{_emailServiceOptions.FromEmail}</b> <br>"+
                          $"Sender name: <b>{_emailServiceOptions.FromName}</b> <br>"+
                          "<p>" +_emailServiceOptions.Body+"</p>"
                };

                using var client = new SmtpClient();
                client.CheckCertificateRevocation = false;
                client.Connect(_emailServiceOptions.Host,
                               _emailServiceOptions.Port,
                               _emailServiceOptions.UseSSL);
                client.Authenticate(_emailServiceOptions.AuthenticationLogin,
                                    _emailServiceOptions.AuthenticationPassword);
                client.Send(message);
                client.Disconnect(true);
                return (true, "E-mail was successfully sent");
            }
            catch (Exception exception)
            {
                return (false, exception.Message);
            }           
            
        }
    }
}
