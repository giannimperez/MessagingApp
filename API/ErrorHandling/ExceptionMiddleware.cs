using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace API.ErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        // every request goes through this try catch block
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                var errorGuid = Guid.NewGuid();
                Log.Error($"Error Guid: {errorGuid} \n{ex}");
  
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new CustomException(context.Response.StatusCode, $"Internal server error: {errorGuid}").MessageFormatted);
            }
        }
    }
}
