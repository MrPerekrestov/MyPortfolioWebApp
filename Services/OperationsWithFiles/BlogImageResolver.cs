using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.Services.OperationsWithFiles.BlogImagesResolverHelpers;

namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public class BlogImageResolver : IBlogImageResolver
    {
        private readonly IBlogImageResolverHelper _helper;
        private readonly ILogger<BlogImageResolver> _logger;
        private readonly IWebHostEnvironment _env;

        public BlogImageResolver(
            IBlogImageResolverHelper helper,
            IWebHostEnvironment env,
            ILogger<BlogImageResolver> logger)
        {
            _helper = helper;
            _logger = logger;
            _env = env;
        }
        private void LogInformation(string info)
        {
            if (_env.IsDevelopment())
            {
                _logger?.LogInformation(info);
            }
        }
        public void Resolve(int blogPostId)
        {
            if (!_helper.BlogPostsDirectoryExists())
            {
                LogInformation("BlogPosts base derictory does not exist. Creating...");
                _helper.CreatePostsDicrectory();
                LogInformation("BlogPosts base derictory was created");
            }

            if (!_helper.CurrentBlogPostDirectoryExists(blogPostId))
            {
                LogInformation($"BlogPosts/{blogPostId} derictory does not exist. Creating...");
                _helper.CreateImagesDirectory(blogPostId);
                LogInformation($"BlogPosts/{blogPostId} derictory was created");
            }

            if (!_helper.ImagesDirectoryExists(blogPostId))
            {
                LogInformation($"Images directory for project {blogPostId} does not exist. Creating...");
                _helper.CreateImagesDirectory(blogPostId);
                LogInformation($"Images directory was created");
            }

            if (!_helper.TimeStampFileExist(blogPostId))
            {
                LogInformation($"Blog post time stamp file does not exist");  
                LogInformation($"Creating all images and time stamps...");
                _helper.CreateAllImagesAndTimeStamps(blogPostId);
                LogInformation($"Images and time stamps were created");
                _helper.UpdateTimeStamp(blogPostId);
                LogInformation($"Blog images time stamp was updated");
            }
            else
            {
                LogInformation($"Blog post time stamp file exists. Checking for timestamp matching...");
                if (!_helper.TimeStampMatches(blogPostId))
                {
                    LogInformation($"Blog posts time stamp does not match. Updating images...");
                    _helper.UpdateImagesAndTimeStamps(blogPostId);
                    LogInformation($"Images were updated");
                    LogInformation($"Updating blog images time stamp..");
                    _helper.UpdateTimeStamp(blogPostId);
                    LogInformation($"Blog images time stamp was updated");
                }
                else
                {
                    LogInformation($"Blog posts time stamp metched.");
                }
            }
        }
    }
}
