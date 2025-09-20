using SmartShop.API.Models.Responses;

namespace SmartShop.API.Helpers
{
    public static class ResponseFactory
    {
        public static ApplicationResponse<T> CreateErrorResponse<T>(string message, string field, string errorMessage, int statusCode)
        {
            return new ApplicationResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<ErrorDetail>
                {
                    new ErrorDetail { Field = field, Message = errorMessage }
                },
                StatusCode = statusCode
            };
        }

        public static ApplicationResponse<T> CreateSuccessResponse<T>(T data, string message, int statusCode)
        {
            return new ApplicationResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }
    }
} 