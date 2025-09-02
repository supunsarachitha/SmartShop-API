namespace SmartShop.API.Models.Responses
{
    public class ApplicationResponse<T>
    {
        public bool Success { get; set; }           // Indicates success or failure
        public string Message { get; set; }         // Human-readable message
        public T Data { get; set; }                 // Actual payload (generic)
        public List<ErrorDetail> Errors { get; set; } // Optional error details
        public int? StatusCode { get; set; }        // Optional HTTP status code
    }

    public class ErrorDetail
    {
        public string Field { get; set; }           // Field that caused the error
        public string Message { get; set; }         // Error message
    }
}
