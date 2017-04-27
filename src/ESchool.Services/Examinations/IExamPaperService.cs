using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IExamPaperService : IService
    {
        Task<ExamPaperDto> GetAsync(int id);

        Task<IPagedList<ExamPaperDto>> GetListAsync(int page, int size);

        Task<ErrorCode> CreateAsync(string examPaperName, int[] questionIds);

        Task<ErrorCode> UpdateAsync(ExamPaper entity, int[] questionIds);

        Task<ErrorCode> DeleteAsync(int id);
    }
}
