using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MyPortfolioWebApp.DbContexts.PortfolioDbContext;
using MyPortfolioWebApp.Services.OperationsWithFiles;
using MyPortfolioWebApp.Services.ProjectsCleaner;
using MyPortfolioWebApp.Services.ProjectsRepository;
using NUglify.JavaScript;

namespace MyPortfolioWebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<PortfolioContext>(options =>
            {
                var connectionString = _configuration.GetConnectionString("portfolio");
                options.UseMySQL(connectionString); 
            });
            services.AddTransient<IProjectsRepository, ProjectsRepositoryEF>();   

            //  add wwwroot/Projects cleaner which delete unused folders
            services.AddHostedService(serviceProvider => {
                using var scope = serviceProvider.CreateScope();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var logger = scope.ServiceProvider.GetService<ILogger<ProjectsCleanerService>>();

                var portfolioDbContextOptions = new DbContextOptionsBuilder<PortfolioContext>()
                        .UseMySQL(config.GetConnectionString("portfolio"))
                        .Options;
                var portfolioContext = new PortfolioContext(portfolioDbContextOptions);
                var repositoryType =scope.ServiceProvider.GetService<IProjectsRepository>().GetType();
                var repo = Activator.CreateInstance(repositoryType, portfolioContext) as IProjectsRepository;                      
                
                return new ProjectsCleanerService(env, config, repo, logger);
            });

            services.AddTransient<IProjectFilesResolver, ProjectFilesResolver>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });     
            
            services.AddWebOptimizer(pipeline=> {                
                pipeline.AddCssBundle("/css/portfolio.min.css", "css/*.css");                
                pipeline.AddJavaScriptBundle(
                        "/js/portfolio.min.js",
                        "js/ProjectsLinkClickBabel.js",
                        "js/SendMail.js",
                        "js/Layout.js",
                        "js/About.js",
                        "js/Projects.js"
                        );    
            });

            services.AddMvc(options =>
            {                
                options.EnableEndpointRouting = false;                
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Home/Error");
            app.UseSwagger();            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc(configureRouters =>
            {
                configureRouters.MapRoute("default", "{controller=Home}/{action=About}");
            });
        }
    }
}
