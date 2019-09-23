using MyPortfolioWebApp.DatabaseManager.DatabaseService.ProjectViewerReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.ProjectsRepository
{
    public interface IProjectsRepository
    {
        IEnumerable<ProjectInfo> GetProjectsInfo();
        ProjectInfo GetProjectInfo(int projectId);
        ProjectInfo GetProjectInfo(string projectName);
        IEnumerable<ImageInfo> GetImagesInfo(string projectName);
        IEnumerable<ImageInfo> GetImagesInfo(int projectId);
        byte[] GetImage(int projectId, int imageId);

        string GetHtml(int projectId);
       
        bool CheckIfProjectExists(string projectName);
        bool CheckIfProjectExists(int projectId);
    }
}
