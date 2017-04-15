using System.Threading.Tasks;
using ESchool.Domain.Enums;

namespace ESchool.Services
{
    public interface IService
    {
        Task<ErrorCode> CommitAsync();
    }
}
