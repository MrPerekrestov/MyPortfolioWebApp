using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using NUglify.Helpers;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.OperationsWithFiles.BlogImagesResolverHelpers
{
    public class BlogImageResolverHelper : IBlogImageResolverHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBlogPostsRepository _repo;
        private readonly string _blogPostsPath;
        private readonly ILogger<BlogImageResolverHelper> _logger;

        public BlogImageResolverHelper(
            IWebHostEnvironment env,
            IBlogPostsRepository repo,
            ILogger<BlogImageResolverHelper> logger)
        {
            _env = env;
            _repo = repo;
            _blogPostsPath = Path.Combine(_env.WebRootPath, "Blog", "BlogPosts");
            _logger = logger;
        }
        private void LogInformation(string info)
        {
            if (_env.IsDevelopment())
            {
                _logger?.LogInformation(info);
            }
        }

        public bool BlogPostsDirectoryExists() =>
            Directory
            .Exists(_blogPostsPath);
       
        public void CreatePostsDicrectory() =>
            Directory
            .CreateDirectory(_blogPostsPath);

        public bool CurrentBlogPostDirectoryExists(int blogPostId) =>
            Directory
            .Exists(Path.Combine(_blogPostsPath, blogPostId.ToString()));

        public void CreateCurrentBlogPostDirectory(int blogPostId) =>
            Directory
            .CreateDirectory(Path.Combine(_blogPostsPath, blogPostId.ToString()));

        public bool TimeStampFileExist(int blogPostId) =>
            File.Exists(Path.Combine(_blogPostsPath, blogPostId.ToString(), "images.tmstmp"));
        public void CreateImagesTimeStamp(int blogPostId) =>
            File.WriteAllText(
                Path.Combine(_blogPostsPath, blogPostId.ToString(), "images.tmstmp"),
                _repo.GetBlogPostImagesChangedDate(blogPostId).ToString()
                );
        public bool TimeStampMatches(int blogPostId)
        {
            var currentTimeStamp = _repo
                .GetBlogPostImagesChangedDate(blogPostId)
                .ToString();

            var timeStampFromFile = File.ReadAllText(
                Path.Combine(_blogPostsPath, blogPostId.ToString(), "images.tmstmp"));

            return currentTimeStamp.Equals(timeStampFromFile);
        }

        public bool ImagesDirectoryExists(int blogPostId) =>
            Directory
            .Exists(Path.Combine(_blogPostsPath, blogPostId.ToString(), "images"));

        public void CreateImagesDirectory(int blogPostId) =>
            Directory
            .CreateDirectory(Path.Combine(_blogPostsPath, blogPostId.ToString(), "images"));

        public void CreateAllImagesAndTimeStamps(int blogPostId) =>
            _repo.GetBlogImages(blogPostId)
                .ForEach(imageInfo =>
                {
                    LogInformation(
                        $"Creating image {imageInfo.Id}.{imageInfo.Extension} for blogpost {blogPostId} ...");

                    var imageBytes = _repo.GetImage(blogPostId, imageInfo.Id);

                    var imagePath = Path.Combine(
                        _blogPostsPath,
                        blogPostId.ToString(),
                        "images",
                        $"{imageInfo.Id}.{imageInfo.Extension}");

                    File.WriteAllBytes(imagePath, imageBytes);

                    LogInformation($"Image was crated");

                    var imageTimeStampPath = Path.Combine(
                        _blogPostsPath,
                        blogPostId.ToString(),
                        "images",
                        $"{imageInfo.Id}.tmstmp");

                    File.WriteAllText(imageTimeStampPath, imageInfo.TimeChanged.ToString());

                    LogInformation($"Timestamp was created");
                });
        public void UpdateTimeStamp(int blogPostId) =>
            File.WriteAllText(
                Path.Combine(_blogPostsPath, blogPostId.ToString(), "images.tmstmp"),
                _repo.GetBlogPostImagesChangedDate(blogPostId).ToString()
        );

        public void UpdateImagesAndTimeStamps(int blogPostId) =>
             _repo.GetBlogImages(blogPostId)
                .ForEach(imageInfo =>
                {
                    LogInformation(
                         $"Checking image {imageInfo.Id}.{imageInfo.Extension} for blogpost {blogPostId} ...");

                    var imageTimeStampPath = Path.Combine(
                        _blogPostsPath,
                        blogPostId.ToString(),
                        "images",
                        $"{imageInfo.Id}.tmstmp");

                    if (!File.Exists(imageTimeStampPath))
                    {
                        _logger?.LogInformation($"File {imageInfo.Id}.tmstmp does not exist...");
                        var imageBytes = _repo.GetImage(blogPostId, imageInfo.Id);

                        var imagePath = Path.Combine(
                            _blogPostsPath,
                            blogPostId.ToString(),
                            "images",
                            $"{imageInfo.Id}.{imageInfo.Extension}");

                        File.WriteAllBytes(imagePath, imageBytes);
                        LogInformation($"Image was crated");
                        File.WriteAllText(imageTimeStampPath, imageInfo.TimeChanged.ToString());
                        LogInformation($"Timestamp was created");
                    }
                    else
                    {
                        var timeStamp = File.ReadAllText(imageTimeStampPath);
                        LogInformation($"File {imageInfo.Id}.tmstmp exists...");
                        if (!timeStamp.Equals(imageInfo.TimeChanged.ToString()))
                        {
                            LogInformation(
                                  $"Timestamp {timeStamp} does not match {imageInfo.TimeChanged.ToString()}...");

                            var imageBytes = _repo.GetImage(blogPostId, imageInfo.Id);

                            var imagePath = Path.Combine(
                                _blogPostsPath,
                                blogPostId.ToString(),
                                "images",
                                $"{imageInfo.Id}.{imageInfo.Extension}");

                            File.WriteAllBytes(imagePath, imageBytes);
                            LogInformation($"Image was crated");

                            File.WriteAllText(imageTimeStampPath, imageInfo.TimeChanged.ToString());
                            LogInformation($"Timestamp was updated");
                        }
                    }
                });
    }
}
