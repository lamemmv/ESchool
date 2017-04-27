using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IExamPaperService : IService
    {
        Task<ExamPaperDto> GetAsync(int id);

        Task<IPagedList<ExamPaperDto>> GetListAsync(int page, int size);

        Task<ExamPaper> CreateAsync(ExamPaper entity, IList<int> questionIds);

        Task<int> UpdateAsync(ExamPaper entity, IList<int> questionIds);

        Task<int> DeleteAsync(int id);
    }
}
