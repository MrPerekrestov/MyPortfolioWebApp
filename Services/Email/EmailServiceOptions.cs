using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit.Text;

namespace MyPortfolioWebApp.Services.Email
{
    public class EmailServiceOptions
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public TextFormat TextFormat { get; set; }
        public string AuthenticationLogin { get; set; }
        public string AuthenticationPassword { get; set; }
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool UseSSL { get; set; } = false;
    }
}
