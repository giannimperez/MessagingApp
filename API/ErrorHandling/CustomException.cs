namespace API.ErrorHandling
{
    public class CustomException
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }


        public CustomException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }


    }
}
