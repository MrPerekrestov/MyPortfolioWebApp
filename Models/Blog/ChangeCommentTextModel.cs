using System.ComponentModel.DataAnnotations;

namespace MyPortfolioWebApp.Models.Blog
{
    public class ChangeCommentTextModel
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}
