using System.Net;

namespace SocialX.Core.DTO.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        private ApiResponse() { }

       

        public static ApiResponse<T> SuccessResponse(
            T? data,
            string? message = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> SuccessResponse(
            string? message = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return SuccessResponse(default, message, statusCode);
        }

        public static ApiResponse<T> FailureResponse(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }

      
        public static ApiResponse<T> FailureResponse(
            string message,
            List<string> errors,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
}
