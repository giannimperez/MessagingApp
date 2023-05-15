using System;

namespace API.ErrorHandling
{
    public class CustomException : Exception
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string JsonMessage { get; set; }

        public CustomException(int statusCode, string message) 
            : base(message)
        {
            StatusCode = statusCode;
            Message = base.Message;
            JsonMessage = $"{{\"Message\":\"{Message}\"}}";
        }



        public CustomException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            //Details = details;
        }

    }
}
