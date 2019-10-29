using MyPortfolioWebApp.DbContexts.BlogDbContext;

namespace MyPortfolioWebApp.Services.CommentServices
{
    public interface ICommentManager
    {
        (bool success, string message) AddComment(Comment comment);
        (bool success, string message) DeleteComment(int commentId);
        string GetCommentOwner(int commentId);
        (bool success, string message) UpdateComment(int commentId, string commentText);
    }
}