using ESchool.Domain.DTOs;
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

            if (code == ErrorCode.DuplicateEntity)
            {
                return BadRequestErrorDto(code, "Entity is duplicated.");
            }

            return Ok();
        }

        protected IActionResult PutResult(ErrorCode code)
        {
            if (code == ErrorCode.Success)
            {
                return NoContent();
            }

            if (code == ErrorCode.NotFound)
            {
                return NotFound();
            }

            if (code == ErrorCode.DuplicateEntity)
            {
                return BadRequestErrorDto(code, "Entity is duplicated.");
            }

            return Ok();
        }

        protected IActionResult DeleteResult(ErrorCode code)
        {
            if (code == ErrorCode.Success)
            {
                return NoContent();
            }

            return NotFound();
        }

        protected IActionResult BadRequestErrorDto(ErrorCode code, string message, object data = null)
        {
            return BadRequest(Json(new ErrorDto(code, message, data)));
        }
    }
}
