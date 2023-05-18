using System;

namespace API.ErrorHandling
{
    public class CustomException : Exception
    {

        public int StatusCode { get; set; }
        public override string Message { get; }
        public string MessageJson { get; set; }
        public override string StackTrace { get; }

        public CustomException(int statusCode, string message) // change statusCode type to HttpStatusCode
            : base(message)
        {
            StatusCode = statusCode;
            Message = message;
            MessageJson = $"{{\"Message\":\"{Message}\"}}";
        }

        public CustomException(int statusCode, string message = null, string innerException = null) // This is not right
            : base(message)
        {
            StatusCode = statusCode;
            Message = message;
            StackTrace = innerException;
        }
    }
}