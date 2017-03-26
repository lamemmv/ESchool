using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Services.Logs;
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

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Search(DateTime fromData, DateTime toDate, string level, int? page, int? size)
		{
			PopularLogLevel();
			var logs = await _logService.GetLogsAsync(fromData, toDate, level, page ?? DefaultPage, size ?? DefaultSize);

			return View(logs);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var log = await _logService.GetSingleAsync(id);

			if (log != null)
			{
				await _logService.DeleteAsync(log);
			}

			return OkServerResult();
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int[] ids)
		{
			await _logService.DeleteAsync(ids);

			return OkServerResult();
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
