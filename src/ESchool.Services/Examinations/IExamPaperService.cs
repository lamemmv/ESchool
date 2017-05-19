using System.Threading.Tasks;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;

namespace ESchool.Services.Examinations
{
    public interface IExamPaperService : IService
    {
        Task<ExamPaper> GetAsync(int id);

        Task<IPagedList<ExamPaper>> GetListAsync(int page, int size);

        Task<ExamPaper> CreateAsync(ExamPaper entity);

        Task<int> DeleteAsync(int id);
    }
}
