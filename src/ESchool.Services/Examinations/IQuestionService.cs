using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IQuestionService
    {
        Task<Question> FindAsync(int id);

        Task<IEnumerable<Question>> GetListAsync(int page, int size);

        Task<ErrorCode> CreateAsync(Question entity, int[] qtagIds);

        Task<ErrorCode> UpdateAsync(Question entity, int[] qtagIds);

        Task<ErrorCode> DeleteAsync(int id);
    }
}
