using System.Linq;
using ESchool.Services.Constants;
using ESchool.Services.Models;
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
                    select new ApiError(ApiErrorTypes.ViewModel, k, e.ErrorMessage));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
