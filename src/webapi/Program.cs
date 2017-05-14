using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using System.IO;
using System.Reflection;


namespace webapi
{
    public class Program
    {
        public static void Main(
            string[] args)
        {
            // Configure serilog global logger
            // Console logger in aspnet cannot include the timestamp, see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging
            // See  https://nblumhardt.com/2016/03/reading-logger-configuration-from-appsettings-json/
            //      http://blog.getseq.net/asp-net-core-1-0-logging-update/
            //      https://mderriey.github.io/2016/11/18/correlation-id-with-asp-net-web-api/
            //      https://github.com/aspnet/Logging/issues/483
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level} - {SourceContext} - {CorrelationId}] {Message}{Exception}{NewLine}")
                .CreateLogger();

            // Needed to use --port "" style as we use WebHost.CreateDefaultBuilder below which will end up parsing any command line args
            // See https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Extensions.Configuration.CommandLine/CommandLineConfigurationProvider.cs
            var port = 5000;
            if (args != null && args.Length >= 2 && args[0] == "--port") { port = int.Parse(args[1]); }

            var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            Log.Logger.Information($"Version {version}");
            Log.Logger.Information($"About to start on port {port}");
            // See https://github.com/aspnet/MetaPackages/blob/dev/src/Microsoft.AspNetCore/WebHost.cs
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://*:{port}")
                .Build();

            host.Run();
        }
    }
}
