using Microsoft.AspNetCore.Hosting;
using MyPortfolioWebApp.Services.ProjectsRepository.ProjectsRepositoryReturnTypes;
using MyPortfolioWebApp.Services.ProjectsRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public class ProjectFilesResolver : IProjectFilesResolver
    {
        private readonly IWebHostEnvironment _env;   
        private readonly IProjectsRepository _projectsRepository;

        public ProjectFilesResolver(
            IWebHostEnvironment env,        
            IProjectsRepository projectsRepository)
        {
            _env = env;          
            _projectsRepository = projectsRepository;
        }
        public void Resolve(ProjectInfo projectInfo)
        {
            var id = projectInfo.Id;           
            var projectDirectory = Path.Combine(_env.WebRootPath, "Projects", id.ToString());
            var imagesDirectoryPath = Path.Combine(projectDirectory, "images");
            var projectImagesChangedPath = Path.Combine(projectDirectory, "images.tmstmp");
            var images = _projectsRepository.GetImagesInfo(id);

            //if project files directory does not exist => create all files from scratch
            if (!Directory.Exists(projectDirectory))
            {
                Directory.CreateDirectory(projectDirectory);
                File.WriteAllText(projectImagesChangedPath, projectInfo.ImagesChanged.ToString());
                Directory.CreateDirectory(imagesDirectoryPath);
                foreach (var image in images)
                {
                    var imagePath = Path.Combine(imagesDirectoryPath, $"{image.Id}{image.Extension}");
                    var imageData = _projectsRepository.GetImage(
                                        projectId: id,
                                        imageId: image.Id);
                    File.WriteAllBytes(imagePath, imageData);
                    var imageTimeStampPath = Path.Combine(imagesDirectoryPath, $"{image.Id}.tmstmp");
                    File.WriteAllText(imageTimeStampPath, image.TimeStamp.ToString());
                }
                return;
            }
            var currentImagesChangedDate = Convert.ToDateTime(File.ReadAllText(
                Path.Combine(projectDirectory, "images.tmstmp")));

            //if project folder has outdated files => check timestamp of each file and replace with newer if is old
            if (!projectInfo.ImagesChanged.Equals(currentImagesChangedDate))
            {
                var importaintFiles = new List<string>();
                foreach (var image in images)
                {
                    var imageTimeStampPath = Path.Combine(imagesDirectoryPath, $"{image.Id}.tmstmp");
                    if (!File.Exists(imageTimeStampPath))
                    {
                        var imagePath = Path.Combine(imagesDirectoryPath, $"{image.Id}{image.Extension}");
                        var imageData = _projectsRepository.GetImage(
                                            projectId: id,
                                            imageId: image.Id);
                                            
                        File.WriteAllBytes(imagePath, imageData);
                        File.WriteAllText(imageTimeStampPath, image.TimeStamp.ToString());
                    }
                    else
                    {
                        var currentImageTimeStamp = Convert.ToDateTime(File.ReadAllText(imageTimeStampPath));
                        if (!currentImageTimeStamp.Equals(image.TimeStamp))
                        {
                            var imagePath = Path.Combine(imagesDirectoryPath, $"{image.Id}{image.Extension}");
                            var imageData = _projectsRepository.GetImage(
                                                projectId: id,
                                                imageId: image.Id);
                                               
                            File.WriteAllBytes(imagePath, imageData);
                            File.WriteAllText(imageTimeStampPath, image.TimeStamp.ToString());
                        }
                    }

                    importaintFiles.Add($"{image.Id}{image.Extension}");
                    importaintFiles.Add($"{image.Id}.tmstmp");
                }
                File.WriteAllText(projectImagesChangedPath, projectInfo.ImagesChanged.ToString());

                //delete useless files
                var files = Directory.GetFiles(imagesDirectoryPath);
                foreach (var file in files)
                {
                    if (!importaintFiles.Contains(file.Split('\\').LastOrDefault()))
                    {
                        var deletePath = Path.Combine(imagesDirectoryPath, file);
                        File.Delete(deletePath);
                    }
                }
            }
        }
    }
}
