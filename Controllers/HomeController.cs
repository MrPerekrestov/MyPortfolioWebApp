using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.Attributes;
using MyPortfolioWebApp.DatabaseManager.DatabaseService;
using MyPortfolioWebApp.Extensions;
using MyPortfolioWebApp.Services.ProjectsRepository;

namespace MyPortfolioWebApp.controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectsRepository _projectsRepository;

        public HomeController(IConfiguration configuration, IProjectsRepository projectsRepository)
        {
            _configuration = configuration;
            _projectsRepository = projectsRepository;
        }

        [Route("")]
        [Route("About")]
        [HttpGet]
        public IActionResult About()
        {          
            return View();
        }

        [HttpGet]        
        [Route("Projects")]
        [Route("Home/Projects")]
        public IActionResult Projects()
        {
            var connectionString = _configuration.GetConnectionString("portfolio");

            var projects = _projectsRepository
                .GetProjectsInfo()
                .OrderBy(proj => proj.Created)
                .Reverse()
                .ToArray();
          
            return View(projects);
        }
        [HttpGet]
        [Route("home/error")]
        public IActionResult Error()
        {
            return View("About");
        }
       
    }
}
