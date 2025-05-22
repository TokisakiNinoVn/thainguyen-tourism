using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Middlewares
{

    public class CorsMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var domainVercel = Environment.GetEnvironmentVariable("CORS_ORIGIN_FRONTEND_PRDUCTION_VERCEL") ?? throw new InvalidOperationException("JWT_KEY is not configured.");
            var domainDevV1 = Environment.GetEnvironmentVariable("CORS_ORIGIN_FRONTEND_DEVELOPMENT_V1");
            var domainDevV2 = Environment.GetEnvironmentVariable("CORS_ORIGIN_FRONTEND_DEVELOPMENT_V2");
            var domainNgrock = Environment.GetEnvironmentVariable("CORS_ORIGIN_NGROK");

            var allowedOrigins = new[]
            {
                domainVercel,
                domainDevV1,
                domainDevV2,
                domainNgrock
            };

            var origin = context.Request.Headers["Origin"].ToString();
            var response = context.Response;

            Console.WriteLine($"CORS middleware set headers for: {origin}");

            if (!string.IsNullOrEmpty(origin) && allowedOrigins.Contains(origin))
            {
                response.Headers["Access-Control-Allow-Origin"] = origin;

                // Chỉ thêm nếu chưa có, tránh 'true, true'
                if (!response.Headers.ContainsKey("Access-Control-Allow-Credentials"))
                {
                    response.Headers["Access-Control-Allow-Credentials"] = "true";
                }

                response.Headers["Access-Control-Allow-Headers"] =
                    "Authorization, Content-Type, Accept, ngrok-skip-browser-warning";

                response.Headers["Access-Control-Allow-Methods"] =
                    "GET, POST, PUT, DELETE, OPTIONS";

                response.Headers["Vary"] = "Origin";
            }

            // Handle preflight OPTIONS request
            if (context.Request.Method == HttpMethods.Options)
            {
                response.StatusCode = StatusCodes.Status204NoContent;
                await response.CompleteAsync();
                return;
            }

            await _next(context);
        }
    }

}
