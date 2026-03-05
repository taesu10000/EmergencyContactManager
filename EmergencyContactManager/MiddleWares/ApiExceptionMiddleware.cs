using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmergencyContactManager.MiddleWares
{
    public sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApiExceptionMiddleware> logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext http)
        {
            try
            {
                await next(http);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");

                var (status, title) = ex switch
                {
                    CreateContactException => (StatusCodes.Status400BadRequest, "Invalid input"),
                    DeserializationException => (StatusCodes.Status415UnsupportedMediaType, "Unsupported format"),
                    NullEntityException => (StatusCodes.Status422UnprocessableEntity, "Parse failed"),
                    _ => (StatusCodes.Status500InternalServerError, "Server error"),
                };

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = ex is AppException ? ex.Message : "Unexpected error occurred."
                };

                http.Response.StatusCode = status;
                http.Response.ContentType = "application/problem+json";

                await http.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
