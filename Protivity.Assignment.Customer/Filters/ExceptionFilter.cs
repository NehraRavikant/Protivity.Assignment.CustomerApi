using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Protivity.Assignment.CustomerApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> _logger)
        {
            logger = _logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "An unhandled exception occurred during operation.");

            var result = new ObjectResult(new
            {
                StatusCode = 500,
                Message = "Internal server error occurred during operation.",
                Details = context.Exception.Message,
            })
            {
                StatusCode = 500,
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
