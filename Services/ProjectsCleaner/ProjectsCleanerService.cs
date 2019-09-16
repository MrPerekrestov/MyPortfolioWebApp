using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.DatabaseManager.DatabaseService;
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

        public ProjectsCleanerService(IWebHostEnvironment env, IConfiguration configuration, ILogger<ProjectsCleanerService> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cleaningFrequency = _configuration.GetValue<int>("CleaningFrequency");
            _timer = new Timer((obj)=> {
                DeleteUnusedProjects();
                _logger.LogInformation($"Projects were cleaned at {DateTime.Now.ToString()}");
                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(cleaningFrequency));
            
            return Task.CompletedTask;
        }
        private void DeleteUnusedProjects()
        {
            var projectsPath = Path.Combine(_env.WebRootPath, "Projects");
            var connectionString = _configuration.GetConnectionString("portfolio");

            if (!Directory.Exists(projectsPath)) return;

            var projectIds = ProjectViewer
                .GetProjectsInfo(connectionString)
                .Select(projectInfo=>projectInfo.Id.ToString())
                .ToArray();

            var projectsDirectories = Directory.GetDirectories(projectsPath);
            foreach(var directory in projectsDirectories)
            {
                if (!projectIds.Contains(directory.Split('\\').LastOrDefault()))
                {
                    Directory.Delete(directory, true);
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
            _timer?.Dispose();
        }
    }
}
