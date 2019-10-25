using System;
using System.Collections.Generic;
using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;

namespace MyPortfolioWebApp.Services.OperationsWithFiles.BlogLogoResolverHelpers
{
    public interface IBlogLogoResolverHelper
    {
        void CreateLogoImageFile(string filePath, byte[] content);
        void CreateLogoTimeStampFile(string filePath, DateTime timeStamp);
        byte[] GetLogoBytes(int id);
        string GetLogoFilePath(int id, string extension);
        IEnumerable<LogoInfo> GetLogosInfo();
        LogoInfo GetLogoInfo(int id);
        string GetLogoTimeStampPath(int id);
        bool LogoFileDoesNotExist(string filePath);
        bool TimeStampDoesNotMatch(string timeStampFilePath, DateTime timeStamp);
    }
}