using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Files;
using ESchool.Services.Exceptions;
using ESchool.Services.Files;
using ESchool.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

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
<<<<<<< Updated upstream
            _serverUploadPath = Path.Combine(hostingEnvironment.WebRootPath, options.Value.ServerUploadFolder);
            CreateServerUploadPathDirectory();
=======
            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
            {
                _serverUploadPath = Path.Combine(Directory.GetCurrentDirectory(), options.Value.ServerUploadFolder);
            }
            else
            {
                _serverUploadPath = Path.Combine(hostingEnvironment.WebRootPath, options.Value.ServerUploadFolder);
            }
>>>>>>> Stashed changes
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _fileService.FindAsync(id);

            if (entity != null)
            {
                string path = Path.Combine(_serverUploadPath, entity.FileName);
                Stream fileStream = new FileStream(path, FileMode.Open);

                return File(fileStream, entity.ContentType);
            }

            return NotFound();
        }

        //[HttpPost]
        //public async Task<IActionResult> Post(IFormFile file)
        //{
        //    if (!IsMultipartContentType())
        //    {
        //        return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
        //    }

        //    if (file != null && file.Length > 0)
        //    {
        //        var entity = await _fileService.UploadFileAsync(file, _serverUploadPath);
        //        var code = await _fileService.CreateAsync(entity);

        //        return PostResult(code, entity.Id);
        //    }

        //    return BadRequest();
        //}

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (!IsMultipartContentType())
            {
                return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
            }

            if (file != null && file.Length > 0)
            {
                var entity = ToBlob(file);

<<<<<<< Updated upstream
                await _fileService.CreateAsync(entity);
                var fileDto = await _fileService.UploadFileAsync(file, entity);

                return Created("Post", fileDto);
=======
                // Return byte[].
                Stream inputStream = file.OpenReadStream();
                BinaryReader binaryReader = new BinaryReader(inputStream);
                byte[] content = binaryReader.ReadBytes((int)file.Length);

                return Created("Post", new { Id = entity.Id, Content = content });
>>>>>>> Stashed changes
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _fileService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Blob Id.");
        }

        private void CreateServerUploadPathDirectory()
        {
            if (!Directory.Exists(_serverUploadPath))
            {
                Directory.CreateDirectory(_serverUploadPath);
            }
        }

        private bool IsMultipartContentType()
        {
            string contentType = HttpContext.Request.ContentType;

            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private Blob ToBlob(IFormFile file)
        {
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string newFileName = _fileService.GetRandomFileName(fileName);

            return new Blob
            {
                FileName = newFileName,
                ContentType = file.ContentType,
                Path = Path.Combine(_serverUploadPath, newFileName),
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}
