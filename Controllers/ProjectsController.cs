using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPortfolioWebApp.DatabaseManager.DatabaseService;
using Google.Protobuf.WellKnownTypes;
using MyPortfolioWebApp.Services.OperationsWithFiles;
using MyPortfolioWebApp.Extensions;
using MyPortfolioWebApp.Services.ProjectsRepository;

namespace MyPortfolioWebApp.Controllers
{
    public class ProjectsController : Controller
    {           
        private readonly IProjectFilesResolver _filesResolver;
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsController(            
            IWebHostEnvironment env,
            IProjectFilesResolver filesResolver,
            IProjectsRepository projectsRepository)
        {       
           
            _filesResolver = filesResolver;
            _projectsRepository = projectsRepository;
            var webContentPath = env.WebRootPath;
            var fullProjectsPath = Path.Combine(webContentPath, "Projects");

            if (!Directory.Exists(fullProjectsPath))
            {
                Directory.CreateDirectory(fullProjectsPath);
            }
        }
       

        [HttpGet]
        [Route("projects/{id}")]
        public IActionResult GetProject(int id)
        {         
            if (!_projectsRepository.CheckIfProjectExists(id))
            {
                return RedirectToAction("About","Home");
            }

            var projectInfo = _projectsRepository.GetProjectInfo(id);          
            _filesResolver.Resolve(projectInfo); 
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Ok(_projectsRepository.GetHtml(id));
            }
            return View("GetProject",_projectsRepository.GetHtml(id));
        }
    }
}