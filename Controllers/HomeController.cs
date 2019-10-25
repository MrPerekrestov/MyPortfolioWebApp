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
using MyPortfolioWebApp.Models.Blog;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using MyPortfolioWebApp.Services.OperationsWithFiles;
using MyPortfolioWebApp.Services.ProjectsRepository;

namespace MyPortfolioWebApp.controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogLogoResolver _logoResolver;
        private readonly IConfiguration _configuration;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IBlogPostsRepository _blogPostsRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IConfiguration configuration,
            IBlogLogoResolver logoResolver,
            IProjectsRepository projectsRepository,
            IBlogPostsRepository blogPostsRepository,
            ILogger<HomeController> logger)
        {
            _logoResolver = logoResolver;
            _configuration = configuration;
            _projectsRepository = projectsRepository;
            _blogPostsRepository = blogPostsRepository;
            _logger = logger;
        }

        [Route("")]
        [Route("About")]
        [HttpGet]
        public IActionResult About()
        {          
            return View();
        }

    
        [Route("Blog")]
        [HttpGet]
        public IActionResult Blog()
        {
            var resolveResult = _logoResolver.Resolve();

            if (!resolveResult.success)
            {
                _logger.LogWarning($"icons were not resolved: {resolveResult.message}");
                return RedirectToAction("About", "Home");
            }

            _logger.LogInformation(resolveResult.message);

            var posts = _blogPostsRepository
                .GetBlogPosts()
                .ToList();

            var model = new BlogModel
            {
                blogPosts = posts
            };
            return View(model);
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
