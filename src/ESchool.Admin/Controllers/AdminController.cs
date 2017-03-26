using System.Linq;
using System.Net;
using ESchool.DomainModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ESchool.Admin.Controllers
{
	[Route("adminapi/[controller]")]
    public abstract class AdminController : Controller
    {
        protected const int DefaultPage = 1;
        protected const int DefaultSize = 25;

        protected IActionResult OkServerResult()
        {
            return Ok(new ServerResult(HttpStatusCode.OK));
        }

        protected IActionResult NotFoundServerResult()
        {
            return NotFound(new ServerResult(HttpStatusCode.NotFound));
        }

        protected IActionResult BadRequestServerResult(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(m => m.Errors)
                .Select(m => m.ErrorMessage)
                .ToList();

            return BadRequest(new ServerResult(HttpStatusCode.BadRequest, errors));
        }
    }
}
