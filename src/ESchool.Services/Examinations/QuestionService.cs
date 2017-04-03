using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<QTag> _qtagRepository;
        private readonly IRepository<Question> _questionRepository;

        public QuestionService(IRepository<QTag> qtagRepository, IRepository<Question> questionRepository)
        {
            _qtagRepository = qtagRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Question> FindAsync(int id)
        {
            return await _questionRepository.FindAsync(id);
        }

        public async Task<IEnumerable<Question>> GetListAsync(int page, int size)
        {
            return await _questionRepository.Query
                .Sort(o => o.OrderBy(t => t.Id))
                .GetListAsync(page, size);
        }

        public async Task<ErrorCode> CreateAsync(Question entity, int[] qtagIds)
        {
            if (qtagIds != null && qtagIds.Length > 0)
            {
                //entity.QuestionTags = qtagIds.Select(t => new QuestionTag { QTagId = t }).ToList();
                //entity. _qtagRepository.Query().Filter(t => qtagIds.Contains(t.Id))
            }

            await _questionRepository.CreateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> UpdateAsync(Question entity, int[] qtagIds)
        {
            if (qtagIds == null || qtagIds.Length == 0)
            {
                entity.QuestionTags = null;
            }
            else
            {
                var currentQTagIds = entity.QuestionTags.Select(t => t.QTagId).ToList();
            }

            await _questionRepository.UpdateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity != null)
            {
                await _questionRepository.DeleteCommitAsync(entity);
            }

            return ErrorCode.Success;
        }
    }
}
