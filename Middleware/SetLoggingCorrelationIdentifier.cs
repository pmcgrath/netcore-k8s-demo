using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Serilog.Context;


namespace webapi.Middleware
{
    public class SetLoggingCorrelationIdentifier
    {
        private readonly RequestDelegate _next;


        public SetLoggingCorrelationIdentifier(
            RequestDelegate next)
        {
            if (next == null) { throw new ArgumentNullException(nameof(next)); }

            this._next = next;
        }


        public async Task Invoke(
            HttpContext context)
        {
            // See https://github.com/serilog/serilog/wiki/Enrichment
            using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            {
                await this._next(context);
            }
        }
    }


    public static class SetLoggingCorrelationIdentifierExtensions
    {
        public static IApplicationBuilder UseSetLoggingCorrelationIdentifier(
            this IApplicationBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            return builder.Use(next => new SetLoggingCorrelationIdentifier(next).Invoke);
        }
    }
}
