using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain;
using ESchool.Domain.Entities.Systems;
using ESchool.Services.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESchool.Admin.Controllers
{
    public class LogController : AdminController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IEnumerable<Log>> Search(DateTime fromData, DateTime toDate, string level, int? page, int? size)
        {
            PopularLogLevel();

            return await _logService.GetListAsync(fromData, toDate, level, page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var code = await _logService.DeleteAsync(id.Value);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ErrorCode.InvalidIdEntity);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var code = await _logService.DeleteAsync(ids);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ErrorCode.InvalidIdEntity);
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
