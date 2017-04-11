using System.Linq;
using ESchool.Domain.Enums;
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

        protected IActionResult PostResult(ErrorCode code, int entityId)
        {
            if (code == ErrorCode.Success)
            {
                return Created("Post", entityId);
            }

            return Ok(code);
        }

        protected IActionResult PutResult(ErrorCode code)
        {
            if (code == ErrorCode.Success)
            {
                return Accepted();
            }
            else if (code == ErrorCode.NotFound)
            {
                return NotFound();
            }

            return Ok(code);
        }

        protected IActionResult DeleteResult(ErrorCode code)
        {
            if (code == ErrorCode.Success)
            {
                return Accepted();
            }

            return NotFound();
        }

        //protected IActionResult BadRequestErrorCode(ModelStateDictionary modelState)
        //{
        //    var errors = modelState.Values
        //        .SelectMany(m => m.Errors)
        //        .Select(m => m.ErrorMessage)
        //        .ToList();

        //    return BadRequest(new ServerResult(ErrorCode.BadRequest, errors));
        //}
    }
}
