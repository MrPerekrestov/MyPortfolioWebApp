using MyPortfolioWebApp.DbContexts.BlogDbContext;
using System.Linq;


namespace MyPortfolioWebApp.Services.CommentServices
{
    public class CommentManager : ICommentManager
    {
        private readonly BlogContext _dbContext;

        public CommentManager(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }
        public (bool success, string message) AddComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            var result = _dbContext.SaveChanges();
            _dbContext.Entry(comment).GetDatabaseValues();

            if (result > 0)
            {
                return (true, "Comment was succesfuly added");
            }
            return (false, "Comment was not added");
        }
        public (bool success, string message) DeleteComment(int commentId)
        {
            var commentToRemove = _dbContext
                .Comments
                .AsQueryable()
                .Where(comment => comment.CommentId == commentId)
                .FirstOrDefault();

            if (commentToRemove == null)
            {
                return (false, "Comment does not exist");
            }
            _dbContext.Comments.Remove(commentToRemove);
            if (_dbContext.SaveChanges() > 0)
            {
                return (true, "Comment was successfully deleted");
            }
            return (false, "Comment was not deleted");
        }
        public (bool success, string message) UpdateComment(int commentId, string commentText)
        {
            var comment = _dbContext
                .Comments
                .AsQueryable()
                .Where(comment => comment.CommentId == commentId)
                .FirstOrDefault();
            if (comment == null)
            {
                return (false, "Comment does not exist");
            }

            if (comment.CommentText == commentText)
            {
                return (false, "Comment text is the same as previous");
            }

            comment.CommentText = commentText;
            var updateResult = _dbContext.SaveChanges();

            if (updateResult > 0)
            {
                return (true, "Comment was updated");
            }
            return (false, "Comment was not updated");
        }
        public string GetCommentOwner(int commentId) =>
            _dbContext
            .Comments
            .AsQueryable()
            .Where(comment => comment.CommentId == commentId)
            .Select(comment => comment.UserId)
            .FirstOrDefault();

    }
}
