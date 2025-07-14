using System.Net;
using System.Text.Json;
using API.Exceptions;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue the request pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";
            var details = exception.Message;

            if (exception is ApiException apiEx)
            {
                statusCode = apiEx.StatusCode;
                message = apiEx.Message;
                details = apiEx.Response ?? apiEx.Message;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                details = details
            });

            response.StatusCode = statusCode;
            await response.WriteAsync(result);
        }
    }
}
