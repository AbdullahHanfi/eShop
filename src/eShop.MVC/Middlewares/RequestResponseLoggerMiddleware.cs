﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace eShop.MVC.Middelwares
{
    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        private readonly bool _isRequestResponseLoggingEnabled;

        public RequestResponseLoggingMiddleware(IConfiguration config)
        {
            _isRequestResponseLoggingEnabled = config.GetValue("EnableRequestResponseLogging", false);
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            // Middleware is enabled only when the EnableRequestResponseLogging config value is set.
            if (_isRequestResponseLoggingEnabled)
            {
                Console.WriteLine($"HTTP request information:\n" +
                    $"\tMethod: {httpContext.Request.Method}\n" +
                    $"\tId: {httpContext.Connection.RemoteIpAddress}\n" +
                    $"\tPath: {httpContext.Request.Path}\n" +
                    $"\tQueryString: {httpContext.Request.QueryString}\n" +
                    $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
                    $"\tSchema: {httpContext.Request.Scheme}\n" +
                    $"\tHost: {httpContext.Request.Host}\n" +
                    $"\tBody: {await ReadBodyFromRequest(httpContext.Request)}");

                // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
                var originalResponseBody = httpContext.Response.Body;
                using var newResponseBody = new MemoryStream();
                httpContext.Response.Body = newResponseBody;

                // Call the next middleware in the pipeline
                await next(httpContext);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalResponseBody);
            }
            else
            {
                await next(httpContext);
            }
        }

        private static string FormatHeaders(IHeaderDictionary headers) =>
            string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
