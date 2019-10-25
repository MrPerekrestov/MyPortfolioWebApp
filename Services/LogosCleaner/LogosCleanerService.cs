using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.Services.BlogPostsCleaner;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.LogosCleaner
{
    public class LogosCleanerService : IHostedService
    {
        private bool disposed = false;
        private Timer _timer;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogPostsRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LogosCleanerService> _logger;

        public LogosCleanerService(
            IWebHostEnvironment env,
            IBlogPostsRepository repo,
            IConfiguration configuration,
            ILogger<LogosCleanerService> logger)
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
                _logger.LogInformation($"Logos were cleaned at {DateTime.Now.ToString()}");
            },
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromSeconds(cleaningPeriod));

            return Task.CompletedTask;
        }
        public void DeleteUnusedBlogPosts()
        {
            var logosPostsPath = Path.Combine(_env.WebRootPath, "Blog", "Logos");

            var logos = Directory.GetFiles(logosPostsPath);

            var logosInfo = _repo
                .GetLogosInfo()
                .Select(logoinfo => new { LogoId = logoinfo.Id.ToString(), LogoExtenssion = logoinfo.Extension })
                .ToList();
            
            logos.ForEach(logoPath =>
            {                
                var extension = logoPath
                .Split("\\")
                .LastOrDefault()
                .Split(".")
                .LastOrDefault();

                var fileNameWithExtension = logoPath
                .Split("\\")
                .LastOrDefault();
                
                var fileName = fileNameWithExtension.Substring(0, fileNameWithExtension.Length - (extension.Length+1));

                var logoInfoMatched = logosInfo
                    .Where(info =>(extension==info.LogoExtenssion || extension == "tmstmp") &&
                     info.LogoId == fileName)
                    .LastOrDefault()!=null;

                if (!logoInfoMatched)
                {
                    File.Delete(logoPath);
                    _logger.LogInformation($"File {fileName}.{extension} was deleted");
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
        ~LogosCleanerService()
        {
            Dispose(false);
        }
    }
}
