using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyPortfolioWebApp.DatabaseManager.DatabaseService.ProjectViewerReturnTypes;
using MyPortfolioWebApp.DbContexts.PortfolioDbContext;

namespace MyPortfolioWebApp.Services.ProjectsRepository
{
    public class ProjectsRepositoryEF : IProjectsRepository
    {
        private readonly PortfolioContext _db;

        public ProjectsRepositoryEF(PortfolioContext db)
        {
            _db = db;
        }

        public bool CheckIfProjectExists(string projectName) =>
            _db
            .Projects
            .Where(proj => proj.Name == projectName)
            .Count() == 1;

        public bool CheckIfProjectExists(int projectId) =>
            _db
            .Projects
            .Where(proj => proj.Id == projectId)
            .Count() == 1;

        public string GetHtml(int projectId) =>
            _db
            .ProjectsHtml
            .Where(projHtml => projHtml.Id == projectId)
            .Select(projHtml => projHtml.Html)
            .FirstOrDefault();

        public byte[] GetImage(int projectId, int imageId) =>
            _db
            .ProjectsImages
            .Where(imgInfo => imgInfo.Projectid == projectId && imgInfo.Imageid == imageId)
            .Select(imgInfo => imgInfo.Image)
            .FirstOrDefault();

        public IEnumerable<ImageInfo> GetImagesInfo(string projectName) =>
            _db
            .ProjectsImages
            .Where(projImage => projImage.Project.Name == projectName)
            .Select(projImage => new ImageInfo
            {
                Extension = projImage.Extension,
                Id = projImage.Imageid,
                TimeStamp = projImage.Timestamp.Value.DateTime
            });


        public IEnumerable<ImageInfo> GetImagesInfo(int projectId) =>
             _db
            .ProjectsImages
            .Where(projImage => projImage.Projectid == projectId)
            .Select(projImage => new ImageInfo
            {
                Extension = projImage.Extension,
                Id = projImage.Imageid,
                TimeStamp = projImage.Timestamp.Value.DateTime
            });

        public ProjectInfo GetProjectInfo(int projectId) =>
            _db
            .Projects
            .Where(proj => proj.Id == projectId)
            .Select(proj => new ProjectInfo
            {
                Id = proj.Id,
                Name = proj.Name,
                Created = proj.Creationdate,
                Description = proj.Description,
                ImagesChanged = proj.Imageschanged.Value,
                Published = proj.Publishingdate
            }).FirstOrDefault();


        public ProjectInfo GetProjectInfo(string projectName) =>
            _db
            .Projects
            .Where(proj => proj.Name == projectName)
            .Select(proj => new ProjectInfo
            {
                Id = proj.Id,
                Name = proj.Name,
                Created = proj.Creationdate,
                Description = proj.Description,
                ImagesChanged = proj.Imageschanged.Value,
                Published = proj.Publishingdate
            })
            .FirstOrDefault();


        public IEnumerable<ProjectInfo> GetProjectsInfo() =>
            _db
            .Projects
            .Select(proj => new ProjectInfo
            {
                Id = proj.Id,
                Name = proj.Name,
                Created = proj.Creationdate,
                Description = proj.Description,
                ImagesChanged = proj.Imageschanged.Value,
                Published = proj.Publishingdate
            });             
       
    }
}
