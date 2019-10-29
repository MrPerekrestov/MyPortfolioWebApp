using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;
using System.Collections.Generic;

namespace MyPortfolioWebApp.Models.Blog
{
    public class BlogPostModel
    {        
        public string BlogPostHtml { get; set; }      
        public string ReturnUri { get; set; }     
        public bool Authorized { get; set; }
        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public List<BlogCommentInfo> CommentsData { get; set; } 
        public int PostId { get; set; }
    }
}
