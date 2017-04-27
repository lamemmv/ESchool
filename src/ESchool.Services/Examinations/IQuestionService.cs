using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IQuestionService : IService
    {
        Task<IList<int>> GetRandomQuestionsAsync(int qtagId, int numberOfRandomQuestion, int difficultLevel);

        Task<QuestionDto> GetAsync(int id);

        Task<IPagedList<QuestionDto>> GetListAsync(int page, int size);

        Task<Question> CreateAsync(Question entity, string[] qtags);

        Task<int> UpdateAsync(Question entity, string[] qtags);

        Task<int> DeleteAsync(int id);
    }
}
