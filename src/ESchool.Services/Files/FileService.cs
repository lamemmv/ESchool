using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Entities.Files;
using ESchool.Domain.Enums;
using ESchool.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace ESchool.Services.Files
{
    public class FileService : BaseService, IFileService
    {
        public FileService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Blob> FindAsync(int id)
        {
            return await Blobs.FindAsync(id);
        }

        public async Task<ErrorCode> CreateAsync(Blob entity)
        {
            await Blobs.AddAsync(entity);

            return await CommitAsync();
        }

        public async Task<ErrorCode> DeleteAsync(Blob entity)
        {
            Blobs.Remove(entity);

            return await CommitAsync();
        }

        public async Task<Blob> UploadFileAsync(IFormFile file, string serverUploadPath)
        {
            if (!Directory.Exists(serverUploadPath))
            {
                Directory.CreateDirectory(serverUploadPath);
            }

            const int DefaultBufferSize = 80 * 1024;

            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string newFileName = GetRandomFileName(fileName);
            string fullPath = Path.Combine(serverUploadPath, newFileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                Stream inputStream = file.OpenReadStream();

                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, default(CancellationToken));
            }

            return new Blob
            {
                FileName = newFileName,
                ContentType = file.ContentType,
                Path = fullPath,
                CreatedDate = DateTime.UtcNow
            };
        }

        private DbSet<Blob> Blobs
        {
            get
            {
                return _dbContext.Set<Blob>();
            }
        }

        private string GetRandomFileName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return $"{name}_{RandomUtils.Numberic(7)}{extension}";
        }
    }
}
