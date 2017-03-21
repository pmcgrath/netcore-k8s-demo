using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace webapi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }


        public Startup(
            IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug((name, level) =>
                {
                    Console.WriteLine($"***** Name: {name},  Level: {level}");
                    return true;
                });


            app.UseMvc();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            // Add framework services.
            // See https://docs.microsoft.com/en-us/aspnet/core/mvc/models/formatting
            services
                .AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
                //.AddXmlSerializerFormatters();

            services.AddSingleton<IContactRepository, InMemoryContactRepository>();
        }
    }
}
