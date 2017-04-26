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
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace ESchool.Services.Files
{
    public class BlobService : BaseService, IBlobService
    {
        public BlobService(ObjectDbContext dbContext, ILogger<BaseService> logger)
            : base(dbContext, logger)
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

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            Blobs.Remove(entity);

            return await CommitAsync();
        }

        public async Task<Blob> UploadFileAsync(IFormFile file, string serverUploadPath)
        {
            const int DefaultBufferSize = 80 * 1024;

            string fileName = $"{RandomUtils.Numberic(7)}-{ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')}";
            string fullPath = Path.Combine(serverUploadPath, fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                Stream inputStream = file.OpenReadStream();

                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, default(CancellationToken));
            }

            return new Blob
            {
                FileName = fileName,
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
    }
}
