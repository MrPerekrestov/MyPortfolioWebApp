
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Models.Blog
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public string RepliedOnText { get; set; }        
        public string RepliedOnUserName { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
