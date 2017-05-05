using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Systems;
using ESchool.Services.Exceptions;
using ESchool.Services.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESchool.Admin.Controllers
{
    public class LogsController : AdminController
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("{id}")]
        public async Task<Log> Get(int id)
        {
            return await _logService.FindAsync(id);
        }

        [HttpGet]
        public async Task<IPagedList<Log>> Get(DateTime fromData, DateTime toDate, string level, int? page, int? size)
        {
            return await _logService.GetListAsync(fromData, toDate, level, page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _logService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Log Id.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                await _logService.DeleteAsync(ids);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Log Ids.");
        }

        [NonAction]
        public void PopularLogLevel()
        {
            IEnumerable<SelectListItem> logLevels = new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "0", Selected = true },
                new SelectListItem { Text = "Fatal", Value = "1" },
                new SelectListItem { Text = "Error", Value = "2" },
                new SelectListItem { Text = "Warn", Value = "3" },
                new SelectListItem { Text = "Info", Value = "4" },
                new SelectListItem { Text = "Debug", Value = "5" },
                new SelectListItem { Text = "Trace", Value = "6" }
            };

            ViewData["LogLevel"] = new SelectList(logLevels);
        }
    }
}
