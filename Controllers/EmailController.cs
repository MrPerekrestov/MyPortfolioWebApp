using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MyPortfolioWebApp.Models.Email;
using MyPortfolioWebApp.Services.Email;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyPortfolioWebApp.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private readonly string _emailLogin;
        private readonly string _emailPassword;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSSL;
        private readonly string _to;
        private readonly string _subject;

        public EmailController(IConfiguration configuration)
        {
            _emailLogin = configuration.GetValue<string>("MailServiceSettings:Login");
            _emailPassword = configuration.GetValue<string>("MailServiceSettings:Password");
            _host = configuration.GetValue<string>("MailServiceSettings:Host");
            _port = configuration.GetValue<int>("MailServiceSettings:Port");
            _useSSL = configuration.GetValue<bool>("MailServiceSettings:UseSSL");
            _to = configuration.GetValue<string>("MailServiceSettings:To");
            _subject = configuration.GetValue<string>("MailServiceSettings:Subject");
        }

        [HttpPost]
        [Route("Send")]
        public IActionResult Send([FromBody] EmailSendModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                       .Select(er => er.ErrorMessage)
                                                       .Aggregate(
                                                            (aggregatedValue, nextValue) =>
                                                                    aggregatedValue += " "+nextValue);
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { success = false, message = modelErrors });
                }

                var emailServiceOptions = new EmailServiceOptions
                {
                    FromEmail = model.Email,
                    FromName = model.Name,
                    To = _to,
                    Subject = _subject,
                    TextFormat = TextFormat.Html,
                    Body = model.Message,
                    AuthenticationLogin = _emailLogin,
                    AuthenticationPassword = _emailPassword,
                    Host = _host,
                    Port = _port,
                    UseSSL = _useSSL
                };

                var emailService = new EmailService(emailServiceOptions);
                var sendResult = emailService.Send();

                if (sendResult.Success)
                {
                    return Ok(new { success = true, message = sendResult.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = sendResult.Message });

            }
            catch(Exception exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new {success = false, message = exception.Message });
            }           
        }
        
    }
}
