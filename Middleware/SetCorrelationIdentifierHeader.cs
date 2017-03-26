using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;


namespace webapi.Middleware
{
    public class SetCorrelationIdentifierHeader
    {
        private readonly RequestDelegate _next;


        public SetCorrelationIdentifierHeader(
            RequestDelegate next)
        {
            if (next == null) { throw new ArgumentNullException(nameof(next)); }

            this._next = next;
        }


        public async Task Invoke(
            HttpContext context)
        {
            // This is probably not going to be unique ? But it is available on the HttpContext and therefore for all log entries
            context.Response.Headers["X-Correlation-Id"] = context.TraceIdentifier;

            await this._next.Invoke(context);
        }
    }


    public static class SetCorrelationIdentifierHeaderExtensions
    {
        public static IApplicationBuilder UseSetCorrelationIdentifierHeader(
            this IApplicationBuilder builder)
        {
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            return builder.Use(next => new SetCorrelationIdentifierHeader(next).Invoke);
        }
    }
}
