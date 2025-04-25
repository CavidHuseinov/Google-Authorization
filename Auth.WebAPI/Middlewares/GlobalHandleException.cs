using System.Net;
using System.Text.Json;

namespace Bookifa.WebAPI.Middlewares
{
    public class GlobalHandleException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalHandleException> _logger;

        public GlobalHandleException(RequestDelegate next, ILogger<GlobalHandleException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. StackTrace: {StackTrace}", ex.StackTrace);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (exception)
            {
                case ArgumentException _:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                case KeyNotFoundException _:
                    response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                    break;
                case UnauthorizedAccessException _:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                    break;
                case InvalidOperationException _:
                    response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                    break;
            }

            var errorResponse = new
            {
                StatusCode = response.StatusCode,
                Message = exception switch
                {
                    ArgumentException => "Invalid data was entered.",
                    KeyNotFoundException => "The requested resource was not found.",
                    UnauthorizedAccessException => "Unauthorized access attempt.",
                    InvalidOperationException => "An invalid operation was performed.",
                    _ => "An unexpected error occurred. Please try again later."
                },
                Details = exception.Message,
                Inner = exception.InnerException?.Message
            };

            try
            {
                var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                await response.WriteAsync(jsonResponse);
            }
            catch (Exception serializationEx)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync("{\"statusCode\":500,\"message\":\"Server Error: There was a problem creating the response.\"}");
            }
        }
    }
}