using FiapGame.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace FiapGame.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception | Method: {Method} | Path: {Path} | CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Items.TryGetValue("CorrelationId", out var correlationId) ? correlationId : "-"
                );
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Ocorreu um erro inesperado.";

            if (exception is DomainException domainException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = domainException.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                message
            };

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
