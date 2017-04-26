using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Files;
using ESchool.Domain.Enums;
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
        private readonly IFileService _fileService;
        private readonly string _serverUploadPath;

        public FilesController(
            IFileService fileService,
            IOptionsSnapshot<AppSettings> options,
            IHostingEnvironment hostingEnvironment)
        {
            _fileService = fileService;
            _serverUploadPath = Path.Combine(hostingEnvironment.WebRootPath, options.Value.ServerUploadFolder);
        }

        [HttpGet("{id}")]
        public async Task<Blob> Get(int id)
        {
            return await _fileService.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (!IsMultipartContentType())
            {
                return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
            }

            if (file != null && file.Length > 0)
            {
                var entity = await _fileService.UploadFileAsync(file, _serverUploadPath);
                var code = await _fileService.CreateAsync(entity);

                return PostResult(code, entity.Id);
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var entity = await _fileService.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                System.IO.File.Delete(entity.Path);
                var code = await _fileService.DeleteAsync(entity);

                return DeleteResult(code);
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Blob Id.");
        }

        private bool IsMultipartContentType()
        {
            string contentType = HttpContext.Request.ContentType;

            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
