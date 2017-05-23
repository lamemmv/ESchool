using System.Threading.Tasks;
using ESchool.Data.DTOs.Examinations;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;

namespace ESchool.Services.Examinations
{
    public interface IQuestionService : IService
    {
        Task<QuestionDto> GetAsync(int id);

        Task<IPagedList<Question>> GetListAsync(int page, int size);

        Task<Question> CreateAsync(Question entity);

        Task<int> UpdateAsync(Question entity);

        Task<int> DeleteAsync(int id);
    }
}
