using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.DTOs.Files;
using ESchool.Data.Entities.Files;
using ESchool.Services.Exceptions;
using ESchool.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Blob> CreateAsync(Blob entity)
        {
            await Blobs.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException("Blob not found.");
            }

            if (File.Exists(entity.Path))
            {
                File.Delete(entity.Path);
            }

            Blobs.Remove(entity);

            return await CommitAsync();
        }

        public async Task<FileDto> UploadFileAsync(IFormFile file, Blob entity)
        {
            const int DefaultBufferSize = 80 * 1024;
            byte[] content = null;

            using (var fileStream = new FileStream(entity.Path, FileMode.Create))
            {
                Stream inputStream = file.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, default(CancellationToken));

                BinaryReader binaryReader = new BinaryReader(inputStream);
                content = binaryReader.ReadBytes((int)file.Length);
            }

            return new FileDto
            {
                Id = entity.Id,
                Content = content
            };
        }

        public string GetRandomFileName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return $"{name}_{RandomUtils.Numberic(7)}{extension}";
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
