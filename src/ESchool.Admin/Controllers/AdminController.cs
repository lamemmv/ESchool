using ESchool.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    //[Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    //[Authorize]
    [Route("admin/[controller]")]
    public abstract class AdminController : Controller
    {
        protected const int DefaultPage = 1;
        protected const int DefaultSize = 25;

        protected IActionResult BadRequestErrorDto(ErrorCode code, string message)
        {
            return BadRequest(new ApiError(code, message));
        }
    }
}
