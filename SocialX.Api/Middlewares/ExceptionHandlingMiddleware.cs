
using SocialX.Core.DTO.Common;
using SocialX.Core.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace SocialX.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(exception, "Unhandled exception");

            context.Response.ContentType = "application/json";

            ApiResponse<string> response = exception switch
            {
             
                NotFoundException nf =>
                    ApiResponse<string>.FailureResponse(nf.Message, HttpStatusCode.NotFound),

            
                BadRequestException br =>
                    ApiResponse<string>.FailureResponse(
                        br.Message,
                        br.Errors,
                        HttpStatusCode.BadRequest),


                ValidationException ve =>
                    ApiResponse<string>.FailureResponse(
                        ve.Message,
                        HttpStatusCode.BadRequest),


                UnauthorizedAccessException =>
                    ApiResponse<string>.FailureResponse(
                        "Unauthorized",
                        HttpStatusCode.Unauthorized),

                ForbiddenException =>
                    ApiResponse<string>.FailureResponse(
                        "Forbidden",
                        HttpStatusCode.Forbidden),

                ConflictException ce =>
                    ApiResponse<string>.FailureResponse(
                        ce.Message,
                        HttpStatusCode.Conflict),

                _ =>
                    ApiResponse<string>.FailureResponse(
                        "An unexpected error occurred",
                        HttpStatusCode.InternalServerError)
            };

            context.Response.StatusCode = (int)response.StatusCode;


            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                response.Message = exception.Message + "\n" + exception.StackTrace;


                var json = JsonSerializer.Serialize(response,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                await context.Response.WriteAsync(json);
            }
        }
    }
}

