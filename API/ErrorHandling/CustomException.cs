using System;

namespace API.ErrorHandling
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public override string Message { get; }
        public string MessageJson { get; set; }

        public CustomException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
            Message = message;
            MessageJson = $"{{\"Message\":\"{Message}\"}}";
        }
    }
}