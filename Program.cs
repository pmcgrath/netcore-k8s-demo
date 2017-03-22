using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;


namespace webapi
{
    public class Program
    {
        public static void Main(
            string[] args)
        {
            var port = 5000;
            if (args != null && args.Length > 0) { port = int.Parse(args[0]); }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()                                    // PENDING - Do we need this ?
                .UseStartup<Startup>()
                .UseUrls($"http://*:{port}")
                .Build();

            host.Run();
        }
    }
}
