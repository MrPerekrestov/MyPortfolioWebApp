using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyPortfolioWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();                    
                    webBuilder.UseUrls("http://*:5000");
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("emailservicesettings.json", optional:false,reloadOnChange:false);
                    });                    
                    webBuilder.ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddFilter("Microsoft", LogLevel.Error);
                    });
                });
    }
}
