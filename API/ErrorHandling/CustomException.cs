using System;

namespace API.ErrorHandling
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public override string Message { get; }
        public string MessageFormatted { get; set; }

        public CustomException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
            Message = message;
            MessageFormatted = $"{{\"error\": \"{Message}\"}}";
        }

        /// <summary>
        /// Converts <see cref="CustomException" into an anonymous object that represents json./>
        /// </summary>
        /// <returns>An anonymous object that represents json.</returns>
        public object ToJson()
        {
            return new { error = Message };
        }
        
    }
}