using MyPortfolioWebApp.DatabaseManager.DatabaseService.ProjectViewerReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.ProjectsRepository
{
    public interface IProjectsRepository
    {
        IEnumerable<ProjectInfo> GetProjectsInfo(string connectionString);
        ProjectInfo GetProjectInfo(int projectId, string connectionString);
        ProjectInfo GetProjectInfo(string projectName, string connectionString);
        IEnumerable<ImageInfo> GetImagesInfo(string projectName, string connectionString);
        IEnumerable<ImageInfo> GetImagesInfo(int projectId, string connectionString);
        byte[] GetImage(int projectId, int imageId, string connectionString);

        string GetHtml(int projectId, string connectionString);
        DateTime GetHtmlTimeStamp(int projectId, string connectionString);
        bool CheckIfProjectExists(string projectName, string connectionString);
        bool CheckIfProjectExists(int projectId, string connectionString);
    }
}
