using System.Linq;
using ESchool.Domain;
using ESchool.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ESchool.Admin.Controllers
{
    [Route("admin/[controller]")]
    public abstract class AdminController : Controller
    {
        protected const int DefaultPage = 1;
        protected const int DefaultSize = 25;

        protected IActionResult ServerErrorCode(ErrorCode code)
        {
            return Ok(new ServerResult(code));
        }

        protected IActionResult OkErrorCode()
        {
            return Ok(new ServerResult(ErrorCode.Success));
        }

        protected IActionResult NotFoundErrorCode()
        {
            return NotFound(new ServerResult(ErrorCode.NotFound));
        }

        protected IActionResult BadRequestErrorCode(ErrorCode code)
        {
            return BadRequest(new ServerResult(code));
        }

        protected IActionResult BadRequestErrorCode(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(m => m.Errors)
                .Select(m => m.ErrorMessage)
                .ToList();

            return BadRequest(new ServerResult(ErrorCode.BadRequest, errors));
        }
    }
}
