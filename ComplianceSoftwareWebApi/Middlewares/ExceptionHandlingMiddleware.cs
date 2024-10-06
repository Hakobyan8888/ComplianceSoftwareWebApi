using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ComplianceSoftwareWebApi.Middlewares
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
                await _next(context);  // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception details
            _logger.LogError(exception, "An unhandled exception occurred.");

            // Set the status code based on exception type (customize as needed)
            var statusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError // Default for unhandled exceptions
            };

            // Create the error response (using ProblemDetails for structured errors)
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message
            };

            // Set response properties
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Return error response as JSON
            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }

}
