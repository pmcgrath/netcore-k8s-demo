using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
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
            // For .SetBasePath see http://stackoverflow.com/questions/38986736/config-json-not-being-found-on-asp-net-core-startup-in-debug
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
                .AddMvc(options => { options.RespectBrowserAcceptHeader = true; })
                .AddXmlSerializerFormatters();

            services.AddSingleton<IContactRepository>(this.GetContactRepository());
        }


        private IContactRepository GetContactRepository()
        {
            IContactRepository result = null;

            // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration
            switch (this.Configuration["store:type"].ToLower())
            {
                case "inmemory":
                    result = new InMemoryContactRepository();
                    break;

                case "redis":
                    result = new RedisContactRepository(this.Configuration["store:connectionString"]);
                    break;

                default:
                    Log.Logger.Fatal("No store config that we can use found");
                    break;

            }

            return result;
        }
    }
}
