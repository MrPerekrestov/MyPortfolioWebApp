using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using NUglify.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.BlogPostsCleaner
{
    public class BlogPostsCleanerService : IHostedService
    {
        private bool disposed = false;
        private Timer _timer;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogPostsRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlogPostsCleanerService> _logger;

        public BlogPostsCleanerService(
            IWebHostEnvironment env,
            IBlogPostsRepository repo,
            IConfiguration configuration,
            ILogger<BlogPostsCleanerService> logger)
        {
            _env = env;
            _repo = repo;
            _configuration = configuration;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cleaningPeriod = _configuration.GetValue<int>("CleaningPeriod");
            _timer = new Timer((obj) => {
                DeleteUnusedBlogPosts();
                _logger.LogInformation($"Blog posts were cleaned at {DateTime.Now.ToString()}");
            },
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromSeconds(cleaningPeriod));

            return Task.CompletedTask;
        }
        public void DeleteUnusedBlogPosts()
        {
            var blogPostsPath = Path.Combine(_env.WebRootPath, "Blog", "BlogPosts");

            var directories = Directory.GetDirectories(blogPostsPath);

            var blogPostIds = _repo
                .GetBlogPosts()
                .Select(blogPost => blogPost.Id.ToString())
                .ToList();
          
            directories.ForEach(directory =>
            {
                var directoryName = directory.Split("\\").LastOrDefault();
                if (!blogPostIds.Contains(directoryName))
                {
                    Directory.Delete(directory,true);                    
                    _logger.LogInformation($"directory {directory} was deleted");
                }
            });

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _timer?.Dispose();
            }

            disposed = true;
        }
        ~BlogPostsCleanerService()
        {
            Dispose(false);
        }
    }
}
