using System.Text;
using System.Collections.Generic;

namespace FiapGame.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = Guid.NewGuid().ToString();

            context.Items["CorrelationId"] = correlationId;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                // ===== REQUEST =====
                context.Request.EnableBuffering();

                var requestBody = await ReadRequestBody(context.Request);

                _logger.LogInformation(
                    "Request | Method: {Method} | Path: {Path} | Body: {Body}",
                    context.Request.Method,
                    context.Request.Path,
                    requestBody
                );

                // ===== RESPONSE =====
                var originalBody = context.Response.Body;
                using var responseBody = new MemoryStream();

                context.Response.Body = responseBody;

                try
                {
                    await _next(context);

                    var responseText = await ReadResponseBody(context.Response);

                    _logger.LogInformation(
                        "Response | StatusCode: {StatusCode} | Body: {Body}",
                        context.Response.StatusCode,
                        responseText
                    );

                    await responseBody.CopyToAsync(originalBody);
                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Position = 0;

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;

            return body;
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }
    }
}
