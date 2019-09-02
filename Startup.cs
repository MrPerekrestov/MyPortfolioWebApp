using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyPortfolioWebApp
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {            
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
           
            app.UseStaticFiles(new StaticFileOptions()
            {
                //OnPrepareResponse = context =>
                //{
                //    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                //    context.Context.Response.Headers.Add("Expires", "-1");
                //}
            });        
            
            app.UseMvc(configureRouters =>
            {
                configureRouters.MapRoute("default", "{controller=Home}/{action=About}");
            });
        }
    }
}
