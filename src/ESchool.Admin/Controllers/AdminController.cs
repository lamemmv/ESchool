using ESchool.Services.Constants;
using ESchool.Services.Models;
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

        protected IActionResult BadRequestApiError(string source, string message)
        {
            return BadRequest(new ApiError(ApiErrorTypes.ViewModel, source, message));
        }
    }
}
