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

namespace MyPortfolioWebApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IProjectFilesResolver _filesResolver;

        public ProjectsController(IConfiguration configuration, IWebHostEnvironment env, IProjectFilesResolver filesResolver)
        {
            _configuration = configuration;
            _env = env;
            _filesResolver = filesResolver;
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
            var connectionString = _configuration.GetConnectionString("portfolio");
            var projectExist = ProjectViewer.CheckIfProjectExists(id, connectionString);
            if (!ProjectViewer.CheckIfProjectExists(id, connectionString))
            {
                return RedirectToAction("About","Home");
            }

            var projectInfo = ProjectViewer.GetProjectInfo(id, connectionString);          
            _filesResolver.Resolve(projectInfo); 
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Ok(ProjectViewer.GetHtml(id, connectionString));
            }
            return View("GetProject",ProjectViewer.GetHtml(id,connectionString));
        }
    }
}