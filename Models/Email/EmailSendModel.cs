using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Models.Email
{
    public class EmailSendModel
    {  
        [EmailAddress(ErrorMessage = "E-mail has invalid format.")]
        public string Email { get; set; }
        [Required]       
        public string Name { get; set; }
        [MinLength(2, ErrorMessage = "Message is too short.")]
        public string Message { get; set; }
    }
}
