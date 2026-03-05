using Application.Exceptions;
using EmergencyContactManager.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace EmergencyContactManager.Middlewares
{
    public sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApiExceptionMiddleware> logger;
        private readonly IStringLocalizer<LanguagePack> stringLocalizer;
        public ApiExceptionMiddleware(RequestDelegate next,
                                      ILogger<ApiExceptionMiddleware> logger,
                                      IStringLocalizer<LanguagePack> stringLocalizer)
        {
            this.next = next;
            this.logger = logger;
            this.stringLocalizer = stringLocalizer;
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
                    DeserializationException => (StatusCodes.Status422UnprocessableEntity, "Unprocessable entity"),
                    UnsupportedFormatException => (StatusCodes.Status415UnsupportedMediaType, "Unsupported format"),
                    NullEntityException => (StatusCodes.Status422UnprocessableEntity, "Parse failed"),
                    _ => (StatusCodes.Status500InternalServerError, "Server error"),
                };

                var detail = ex is AppException aex
                 ? stringLocalizer[aex.MessageKey, aex.MessageArgs].Value
                 : stringLocalizer["Errors.Unexpected"].Value;


                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = detail
                };

                http.Response.StatusCode = status;
                http.Response.ContentType = "application/problem+json";

                await http.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
