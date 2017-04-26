using System.IO;
using System.Threading.Tasks;
using ESchool.Admin.Attributes;
using ESchool.Services.Files;
using ESchool.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ESchool.Admin.Controllers
{
    public class FilesController : AdminController
    {
        private readonly IBlobService _blobService;
        private readonly string _serverUploadPath;

        public FilesController(
            IBlobService blobService,
            IOptionsSnapshot<AppSettings> options,
            IHostingEnvironment hostingEnvironment)
        {
            _blobService = blobService;
            _serverUploadPath = Path.Combine(hostingEnvironment.WebRootPath, options.Value.ServerUploadFolder);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var entity = await _blobService.UploadFileAsync(file, _serverUploadPath);
                var code = await _blobService.CreateAsync(entity);

                return PostResult(code, entity.Id);
            }

            return BadRequest();
        }
    }
}
