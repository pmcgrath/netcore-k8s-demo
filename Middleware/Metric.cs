using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;


namespace webapi.Middleware
{
    public class Metric
    {
        private readonly RequestDelegate _next;


        public Metric(
            RequestDelegate next)
        {
            if (next == null) { throw new ArgumentNullException(nameof(next)); }

            this._next = next;
        }


        public async Task Invoke(
            HttpContext context)
        {
            // PENDING - What about uncaught exceptions ?
            // PENDING - Promotheus
            var started = DateTime.UtcNow;
            try
            {
                Console.WriteLine($"Started Metric: {context.Request.Method} on {context.Request.Path}");
                await _next.Invoke(context);
            }
            catch
            {
                Console.WriteLine($"Completed Metric: {context.Request.Method} on {context.Request.Path} took {DateTime.UtcNow - started} result is 500");
                throw;
            }
            Console.WriteLine($"Completed Metric: {context.Request.Method} on {context.Request.Path} took {DateTime.UtcNow - started} result is {context.Response.StatusCode}");
        }
    }


    public static class MetricExtensions
    {
        public static IApplicationBuilder UseMetric(
            this IApplicationBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            return builder.Use(next => new Metric(next).Invoke);
        }
    }
}
