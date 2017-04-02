using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
            // Logging
            // See  http://blog.getseq.net/asp-net-core-1-0-logging-update/
            //      https://mderriey.github.io/2016/11/18/correlation-id-with-asp-net-web-api/
            //      https://github.com/aspnet/Logging/issues/483

            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug((category, level) =>
            //     {
            //         Console.WriteLine($"***** Name: {category},  Level: {level}");
            //         return true;
            //     });
            loggerFactory.AddSerilog();

            app.UseMetric();
            app.UseSetLoggingCorrelationIdentifier();
            app.UseSetCorrelationIdentifierHeader();
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

            //services.AddSingleton<IContactRepository, InMemoryContactRepository>();
            services.AddSingleton<IContactRepository, RedisContactRepository>();
        }
    }
}
