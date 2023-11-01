using System.Text.Json;

namespace Protivity.Assignment.CustomerApi.Common.Middleware
{
    public class ExceptionMiddleware
    {
        /// <summary>
        /// variable to hold next middleware
        /// </summary>
        private readonly RequestDelegate _next;


        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle and log exceptions
                await HandleExceptionAsync(context, ex);
            }
        }
        /// <summary>
        /// handle exception
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            string message = "An error occurred.";

            if (exception is ApplicationException appEx)
            {
                /// application exception
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = appEx.Message;
            }
            else if (exception is BadHttpRequestException badReqEx)
            {
                //bad request exception
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                message = badReqEx.Message;
            }
            else if (httpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                //keeping this only for others as well
                message = "Data Validation failed. Errors occurred.";
            }

            var errorResponse = new
            {
                httpContext.Response.StatusCode,
                Message = message
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
