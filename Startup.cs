using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.DbContexts.PortfolioDbContext;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using MyPortfolioWebApp.Services.OperationsWithFiles;
using MyPortfolioWebApp.Services.OperationsWithFiles.BlogLogoResolverHelpers;
using MyPortfolioWebApp.Services.ProjectsRepository;
using React.AspNet;
using MyPortfolioWebApp.Services.CommentServices;
using MyPortfolioWebApp.Services.OperationsWithFiles.BlogImagesResolverHelpers;
using MyPortfolioWebApp.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace MyPortfolioWebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<PortfolioContext>(options =>
            {
                var connectionString = _configuration.GetConnectionString("portfolio");
                options.UseMySQL(connectionString); 
            });

            services.AddDbContextPool<BlogContext>(options =>
            {
                var connectionString = _configuration.GetConnectionString("blog");
                options.UseMySQL(connectionString);
            });

            services.AddTransient<IProjectsRepository, ProjectsRepositoryEF>();
            services.AddTransient<IBlogPostsRepository, BlogPostsRepositoryEF>();
            services.AddTransient<IBlogLogoResolverHelper, BlogLogoResolverHelper>();
            services.AddTransient<IBlogLogoResolver, BlogLogoResolver>();
            services.AddTransient<IProjectFilesResolver, ProjectFilesResolver>();
            services.AddTransient<IProjectFilesResolver, ProjectFilesResolver>();
            services.AddTransient<IBlogImageResolverHelper, BlogImageResolverHelper>();
            services.AddTransient<IBlogImageResolver, BlogImageResolver>();
            services.AddTransient<ICommentManager, CommentManager>();

            services.AddProjectsCleaner();            
            services.AddBlogPostsCleaner();
            services.AddLogosCleaner();

            if (_environment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                });
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Any);
            });

            services.ConfigureWebOptimizer();
            services.ConfigureReact();          
            services.ConfigureAuthentication(_configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
                });
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });           
            app.UseHttpMethodOverride();

            app.Use((context, next) =>
            {
                if (context.Request.Headers["x-forwarded-proto"] == "https")
                {
                    context.Request.Scheme = "https";
                }
                return next();
            });

            app.UseAuthentication();
            
            app.UseStatusCodePagesWithReExecute("/Home/Error");
           
            app.UseReact(conf => {               
                conf.AddScriptWithoutTransform("/components/*.js");                            
                conf.SetAllowJavaScriptPrecompilation(true);
                conf.SetLoadBabel(true);
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("default", "{controller=Home}/{action=About}");
            });          
        }        
    }
}
