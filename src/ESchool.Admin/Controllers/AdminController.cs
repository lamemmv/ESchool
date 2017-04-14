using ESchool.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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
                return NoContent();
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
                return NoContent();
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
