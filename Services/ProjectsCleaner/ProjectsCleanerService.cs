using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.DatabaseManager.DatabaseService;
using MyPortfolioWebApp.Services.ProjectsRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.ProjectsCleaner
{
    public class ProjectsCleanerService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProjectsCleanerService> _logger;
        private readonly IProjectsRepository _projectsRepository;
        private bool disposed = false;

        public ProjectsCleanerService(
            IWebHostEnvironment env,
            IConfiguration configuration,
            IProjectsRepository projectsRepository,
            ILogger<ProjectsCleanerService> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
            _projectsRepository = projectsRepository;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cleaningPeriod = _configuration.GetValue<int>("CleaningPeriod");
            _timer = new Timer((obj)=> {
                DeleteUnusedProjects();
                _logger.LogInformation($"Projects were cleaned at {DateTime.Now.ToString()}");
                },
                state:null,
                dueTime:TimeSpan.Zero,
                period:TimeSpan.FromSeconds(cleaningPeriod));
            
            return Task.CompletedTask;
        }
        private void DeleteUnusedProjects()
        {
            var projectsPath = Path.Combine(_env.WebRootPath, "Projects");        

            if (!Directory.Exists(projectsPath)) return;

            var projectIds = 
                _projectsRepository
                .GetProjectsInfo()
                .Select(projectInfo=>projectInfo.Id.ToString())
                .ToArray();

            var projectsDirectories = Directory.GetDirectories(projectsPath);
            foreach(var directory in projectsDirectories)
            {
                if (!projectIds.Contains(directory.Split('\\').LastOrDefault()))
                {
                    Directory.Delete(directory, recursive:true);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
      
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }
      
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _timer?.Dispose();                
            }

            disposed = true;
        }
        ~ProjectsCleanerService()
        {
            Dispose(false);
        }
    }
}
