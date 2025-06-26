using HelloContainer.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace HelloContainer.Api.Middleware
{
    public class DomainExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DomainExceptionHandlerMiddleware> _logger;

        public DomainExceptionHandlerMiddleware(RequestDelegate next, ILogger<DomainExceptionHandlerMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse();

            switch (exception)
            {
                case ContainerNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = exception.Message;
                    errorResponse.ErrorType = "ContainerNotFound";
                    break;

                case ContainerOverflowException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = exception.Message;
                    errorResponse.ErrorType = "ContainerOverflow";
                    break;

                case InvalidConnectionException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = exception.Message;
                    errorResponse.ErrorType = "InvalidConnection";
                    break;

                case DomainException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = exception.Message;
                    errorResponse.ErrorType = "DomainError";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An unexpected error occurred.";
                    errorResponse.ErrorType = "InternalServerError";
                    _logger.LogError(exception, "An unhandled exception occurred");
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 