using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyPortfolioWebApp.Services.OperationsWithFiles;
using MyPortfolioWebApp.Services.ProjectsCleaner;
using NUglify.JavaScript;

namespace MyPortfolioWebApp
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ProjectsCleanerService>();
            services.AddSingleton<IProjectFilesResolver, ProjectFilesResolver>();
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
