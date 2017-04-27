using System.Threading.Tasks;
using ESchool.Domain.DTOs.Files;
using ESchool.Domain.Entities.Files;
using Microsoft.AspNetCore.Http;

namespace ESchool.Services.Files
{
    public interface IFileService : IService
    {
        Task<Blob> FindAsync(int id);

        Task<Blob> CreateAsync(Blob entity);

        Task<int> DeleteAsync(int id);

        Task<FileDto> UploadFileAsync(IFormFile file, Blob entity);

        string GetRandomFileName(string fileName);
    }
}
