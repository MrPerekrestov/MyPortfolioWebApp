using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.DbContexts.PortfolioDbContext;
using MyPortfolioWebApp.Services.BlogPostsCleaner;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using MyPortfolioWebApp.Services.LogosCleaner;
using MyPortfolioWebApp.Services.ProjectsCleaner;
using MyPortfolioWebApp.Services.ProjectsRepository;
using React.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
       
        public static void AddBlogPostsCleaner(this IServiceCollection services)
        {
            services.AddHostedService(serviceProvider => {
                using var scope = serviceProvider.CreateScope();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var logger = scope.ServiceProvider.GetService<ILogger<BlogPostsCleanerService>>();

                var portfolioDbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                        .UseMySQL(config.GetConnectionString("portfolio"))
                        .Options;
                var portfolioContext = new BlogContext(portfolioDbContextOptions);
                var repositoryType = scope.ServiceProvider.GetService<IBlogPostsRepository>().GetType();
                var repo = Activator.CreateInstance(repositoryType, portfolioContext) as IBlogPostsRepository;

                return new BlogPostsCleanerService(env,  repo, config, logger);
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;                
            })
                .AddCookie()
                .AddFacebook(options =>
                {
                   
                    var appId = configuration.GetValue<string>("AppId");
                    var appSecret = configuration.GetValue<string>("AppSecret");

                    options.AppId = appId;
                    options.AppSecret = appSecret;

                    options.SaveTokens = true;

                });
            services.AddAuthorization();
        }
        public static void ConfigureReact(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
             .AddChakraCore(configure =>
             {
                 configure.DisableFatalOnOOM = true;
             });
        }
        public static void ConfigureWebOptimizer(this IServiceCollection services)
        {
            services.AddWebOptimizer(pipeline => {
                pipeline.AddCssBundle("/css/portfolio.min.css", "css/*.css");
                pipeline.AddJavaScriptBundle(
                        "/js/portfolio.min.js",
                        "/lib/react/umd/react.development.js",
                        "/lib/react-dom/umd/react-dom.production.min.js",
                        "js/ProjectsLinkClickBabel.js",
                        "js/SendMail.js",
                        "js/Layout.js",
                        "js/About.js",
                        "js/Projects.js",
                        "js/Blog.js",
                         "components/*.js"
                        );
            });
        }
        public static void AddProjectsCleaner(this IServiceCollection services)
        {
            services.AddHostedService(serviceProvider => {
                using var scope = serviceProvider.CreateScope();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var logger = scope.ServiceProvider.GetService<ILogger<ProjectsCleanerService>>();

                var portfolioDbContextOptions = new DbContextOptionsBuilder<PortfolioContext>()
                        .UseMySQL(config.GetConnectionString("portfolio"))
                        .Options;
                var portfolioContext = new PortfolioContext(portfolioDbContextOptions);
                var repositoryType = scope.ServiceProvider.GetService<IProjectsRepository>().GetType();
                var repo = Activator.CreateInstance(repositoryType, portfolioContext) as IProjectsRepository;

                return new ProjectsCleanerService(env, config, repo, logger);
            });
        }
        public static void AddLogosCleaner(this IServiceCollection services)
        {
            services.AddHostedService(serviceProvider => {
                using var scope = serviceProvider.CreateScope();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var logger = scope.ServiceProvider.GetService<ILogger<LogosCleanerService>>();

                var portfolioDbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                        .UseMySQL(config.GetConnectionString("portfolio"))
                        .Options;
                var portfolioContext = new BlogContext(portfolioDbContextOptions);
                var repositoryType = scope.ServiceProvider.GetService<IBlogPostsRepository>().GetType();
                var repo = Activator.CreateInstance(repositoryType, portfolioContext) as IBlogPostsRepository;

                return new LogosCleanerService(env, repo, config, logger);
            });
        }
    }
}
