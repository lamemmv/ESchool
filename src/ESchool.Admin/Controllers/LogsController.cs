using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Systems;
using ESchool.Services.Exceptions;
using ESchool.Services.Systems;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class LogsController : AdminController
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("GetLevels")]
        public IList<string> GetLevels()
        {
            return new List<string>
            {
                "All",
                "Fatal",
                "Error",
                "Warn",
                "Info",
                "Debug",
                "Trace"
            };
        }

        [HttpGet("{id}")]
        public async Task<Log> Get(int id)
        {
            return await _logService.FindAsync(id);
        }

        [HttpGet]
        public async Task<IPagedList<Log>> Get(DateTime fromDate, DateTime toDate, string level, int? page, int? size)
        {
            return await _logService.GetListAsync(fromDate, toDate, level, page ?? DefaultPage, size ?? DefaultSize);
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
    }
}
