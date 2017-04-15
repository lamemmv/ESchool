using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IQuestionService : IService
    {
        Task<Question> FindAsync(int id);

        Task<IPagedList<QuestionDto>> GetListAsync(int page, int size);

        Task<ErrorCode> CreateAsync(Question entity, string[] qtags);

        Task<ErrorCode> UpdateAsync(Question entity, string[] qtags);

        Task<ErrorCode> DeleteAsync(int id);
    }
}
