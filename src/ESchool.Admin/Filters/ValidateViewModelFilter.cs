using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESchool.Admin.Filters
{
    public sealed class ValidateViewModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    from kvp in context.ModelState
                    from e in kvp.Value.Errors
                    let k = kvp.Key
                    select new ValidationError("ViewModel", null, k, e.ErrorMessage));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }

    public sealed class ValidationError
    {
        public ValidationError(string type, string code, string source, string message)
        {
            ErrorType = type;
            ErrorCode = code;
            Source = source;
            ErrorMessage = message;
        }

        public string ErrorType { get; }

        public string ErrorCode { get; }

        public string Source { get; }

        public string ErrorMessage { get; }
    }
}
