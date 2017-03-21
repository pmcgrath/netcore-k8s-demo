using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;


namespace webapi.Middleware
{
    public class CorrelationIdentifier
    {
        private readonly RequestDelegate _next;


        public CorrelationIdentifier(
            RequestDelegate next)
        {
            if (next == null) { throw new ArgumentNullException(nameof(next)); }

            this._next = next;
        }


        public async Task Invoke(
            HttpContext context)
        {
            // This is probably not going to be unique, but it does turn up in all the log entries - ASP.NET entries
            context.Response.Headers["Correlation-Identifier"] = context.TraceIdentifier;

            await _next.Invoke(context);
        }
    }


    public static class CorrelationIdentifierExtensions
    {
        public static IApplicationBuilder UseCorrelationIdentifier(
            this IApplicationBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            return builder.Use(next => new CorrelationIdentifier(next).Invoke);
        }
    }
}
