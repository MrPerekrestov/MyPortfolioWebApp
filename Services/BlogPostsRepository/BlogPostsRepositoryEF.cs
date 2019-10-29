using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPortfolioWebApp.Services.BlogPostsRepository
{
    public class BlogPostsRepositoryEF : IBlogPostsRepository
    {
        private readonly BlogContext _dbContext;

        public BlogPostsRepositoryEF(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool LogoExists(int id) =>
            _dbContext.Logos.Where(logo => logo.Id == id).FirstOrDefault() != null;
        public bool BlogExists(int id) =>
            _dbContext.Posts.Where(post => post.Id == id).FirstOrDefault() != null;
        public int GetLastBlogId() =>
            _dbContext.Posts.Select(post => post.Id).Max();

        public IEnumerable<BlogPostInfo> GetBlogPosts() =>
            _dbContext
            .Posts
            .AsQueryable()
            .OrderByDescending(post => post.Published)
            .Select(post => new BlogPostInfo
            {
                Id = post.Id,
                Title = post.Title,
                LogoId = post.LogoId.HasValue ? post.LogoId.Value : 1,
                ImagesChanged = post.ImagesChanged,
                Published = post.Published,
                Description = post.Description
            });
        public IEnumerable<BlogImageInfo> GetBlogImages(int postId) =>
            _dbContext
            .Images
            .AsQueryable()
            .Where(image => image.BlogPostId == postId)
            .Select(image => new BlogImageInfo
            {
                Id = image.ImageId,
                Extension = image.Extension,
                TimeChanged = image.TimeChanged
            });
        public bool ImageExists(int postId, int imageId) =>
            _dbContext
            .Images
            .AsQueryable()
            .Where(image => image.ImageId == imageId && image.BlogPostId == postId)
            .FirstOrDefault() != null;

        public IEnumerable<LogoInfo> GetLogosInfo() =>
            _dbContext
            .Logos
            .AsQueryable()
            .Select(logo => new LogoInfo
            {
                Id = logo.Id,
                Extension= logo.Extension,
                TimeChanged = logo.TimeChanged
            });

        public LogoInfo GetLogoInfo(int id) =>
            _dbContext
            .Logos
            .AsQueryable()
            .Where(logo => logo.Id == id)
            .Select(logo=>new LogoInfo
            {
                Id = logo.Id,
                Extension = logo.Extension,
                TimeChanged = logo.TimeChanged
            })
            .FirstOrDefault();

        public byte[] GetLogoBytes(int id) => 
            _dbContext
            .Logos
            .AsQueryable()
            .Where(logo => logo.Id == id)
            .Select(logo => logo.LogoBytes)
            .FirstOrDefault();

        public string GetBlogPostHtml(int id) =>
            _dbContext
            .Posts
            .AsQueryable()
            .Where(post => post.Id == id)
            .Select(post => post.Html)
            .FirstOrDefault();

        public IEnumerable<BlogCommentInfo> GetComments(int postId) =>
            _dbContext
            .Comments
            .AsQueryable()
            .Where(commment => commment.PostId == postId)
            .Select(comment=>new BlogCommentInfo
                {
                    CommentId = comment.CommentId,
                    CommentText = comment.CommentText,
                    PostedDate = comment.PostedDate,
                    PostId = comment.PostId,
                    RepliedOnText = comment.RepliedOnText,
                    RepliedOnUserName = comment.RepliedOnUserName,
                    UserId = comment.UserId,
                    UserName = comment.UserName
                });
        public IEnumerable<BlogImageInfo> GetImagesInfo(int postId) =>
            _dbContext
            .Images
            .AsQueryable()
            .Where(image => image.BlogPostId == postId)
            .Select(image => new BlogImageInfo
            {
                Id = image.ImageId,
                Extension = image.Extension,
                TimeChanged = image.TimeChanged
            });
        public byte[] GetImage(int postId, int imageId) =>
            _dbContext
            .Images
            .AsQueryable()
            .Where(image => image.BlogPostId == postId && image.ImageId == imageId)
            .Select(image => image.ImageBlob)
            .FirstOrDefault();
        public DateTime GetBlogPostImagesChangedDate(int postId) =>
            _dbContext
            .Posts
            .AsQueryable()
            .Where(post => post.Id == postId)
            .Select(post => post.ImagesChanged)
            .FirstOrDefault();
    }
}
