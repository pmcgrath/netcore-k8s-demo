using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using webapi.Middleware;


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

            app.UseMetric();
            app.UseCorrelationIdentifier();
            app.UseMvc();
        }


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
