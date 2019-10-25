using Microsoft.AspNetCore.Hosting;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.OperationsWithFiles.BlogLogoResolverHelpers
{
    public class BlogLogoResolverHelper : IBlogLogoResolverHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBlogPostsRepository _repository;

        public BlogLogoResolverHelper(IWebHostEnvironment env, IBlogPostsRepository repository)
        {
            _env = env;
            _repository = repository;
        }

        public string GetLogoFilePath(int id, string extension) =>
                Path.Combine(_env.WebRootPath,
                    "Blog", "Logos", $"{id}.{extension}");
        public string GetLogoTimeStampPath(int id) => Path.Combine(_env.WebRootPath,
                    "Blog", "Logos", $"{id}.tmstmp");
        public LogoInfo GetLogoInfo(int id) => _repository.GetLogoInfo(id);
        public IEnumerable<LogoInfo> GetLogosInfo() => _repository.GetLogosInfo();
        public bool LogoFileDoesNotExist(string filePath) => !File.Exists(filePath);
        public void CreateLogoImageFile(string filePath, byte[] content) => File.WriteAllBytes(filePath, content);
        public void CreateLogoTimeStampFile(string filePath, DateTime timeStamp) => File.WriteAllText(filePath, timeStamp.ToString());
        public byte[] GetLogoBytes(int id) => _repository.GetLogoBytes(id);
        public bool TimeStampDoesNotMatch(string timeStampFilePath, DateTime timeStamp) =>
            !File.Exists(timeStampFilePath) ? true :
                File.ReadAllText(timeStampFilePath) == timeStamp.ToString() ? false : true;
    }
}
