namespace AtivaBank.Domain.Response
{
    public class Response<T>
    {
        public bool IsSuccess => string.IsNullOrEmpty(Error);
        public T Data { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public Response(T data, string message)
        {
            Data = data;
            Message = message;
        }

        public Response(string error) 
        {
            Error = error;  
        }

        public static Response<T> SuccessResponse(T data, string message = "") => new Response<T>(data, message);
        public static Response<T> ErrorResponse(T data, string error) => new Response<T>(error);
    }
}
