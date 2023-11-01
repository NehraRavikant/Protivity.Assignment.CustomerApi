using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Protivity.Assignment.CustomerApi.Common;

namespace Protivity.Assignment.CustomerApi.Filters
{
    public class ModelExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// filter
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidModelException modelStateException)
            {
                var errors = new Dictionary<string, string[]>();

                foreach (var error in modelStateException.Errors)
                {
                    var fieldName = error.MemberNames.FirstOrDefault() ?? "FieldNotFound";
                    var errorMessage = error.ErrorMessage;

                    if (!errors.ContainsKey(fieldName))
                    {
                        errors[fieldName] = new string[] { errorMessage };
                    }
                    else
                    {
                        errors[fieldName] = errors[fieldName].Append(errorMessage).ToArray();
                    }
                }

                var result = new ObjectResult(new
                {
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors
                })
                {
                    StatusCode = 400
                };

                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
