using System.Threading.Tasks;
using ESchool.Domain.Entities.Files;
using ESchool.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ESchool.Services.Files
{
    public interface IBlobService : IService
    {
        Task<Blob> FindAsync(int id);

        Task<ErrorCode> CreateAsync(Blob entity);

        Task<ErrorCode> DeleteAsync(int id);

        Task<Blob> UploadFileAsync(IFormFile file, string serverUploadPath);
    }
}
