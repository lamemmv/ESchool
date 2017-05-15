using System.Threading.Tasks;

namespace ESchool.Services.Infrastructure.Tasks
{
    public interface IBackgroundTask
    {
        void Start();

        void Stop();

        Task Execute();
    }
}
