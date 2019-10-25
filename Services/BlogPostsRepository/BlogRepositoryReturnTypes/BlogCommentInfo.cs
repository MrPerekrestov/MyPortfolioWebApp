using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes
{
    public class BlogCommentInfo
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public string RepliedOnText { get; set; }
        public string RepliedOnUserName { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
