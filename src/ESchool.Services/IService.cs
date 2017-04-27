using System.Threading.Tasks;

namespace ESchool.Services
{
    public interface IService
    {
        Task<int> CommitAsync();
    }
}
