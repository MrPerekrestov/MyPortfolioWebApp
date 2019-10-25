using System;
using System.Collections;
using System.Collections.Generic;
using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;

namespace MyPortfolioWebApp.Services.BlogPostsRepository
{
    public interface IBlogPostsRepository
    {
        bool BlogExists(int id);
        IEnumerable<BlogImageInfo> GetBlogImages(int postId);
        IEnumerable<BlogPostInfo> GetBlogPosts();
        int GetLastBlogId();
        bool ImageExists(int postId, int imageId);
        bool LogoExists(int id);
        IEnumerable<LogoInfo> GetLogosInfo();
        LogoInfo GetLogoInfo(int id);
        byte[] GetLogoBytes(int id);
        string GetBlogPostHtml(int id);
        IEnumerable<BlogCommentInfo> GetComments(int postId);
        IEnumerable<BlogImageInfo> GetImagesInfo(int postId);
        byte[] GetImage(int postId, int imageId);
        DateTime GetBlogPostImagesChangedDate(int postId);

    }
}