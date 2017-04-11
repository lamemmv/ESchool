using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

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
            return await _questionRepository.QueryNoTracking
                .Include(q => q.Answers)
                .Filter(q => q.Id == id)
                .GetSingleAsync();
        }

        public async Task<IEnumerable<Question>> GetListAsync(int page, int size)
        {
            return await _questionRepository.QueryNoTracking
                .Include(q => q.Answers)
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
            var updatedEntity = await _questionRepository.Query
                .Include(q => q.Answers)
                .Filter(q => q.Id == entity.Id)
                .GetSingleAsync();

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            updatedEntity.Content = entity.Content;
            updatedEntity.Description = entity.Description;
            updatedEntity.Type = entity.Type;
            updatedEntity.Answers.Clear();

            if (entity.Answers != null)
            {
                foreach (var answer in entity.Answers)
                {
                    updatedEntity.Answers.Add(answer);
                }
            }

            await _questionRepository.UpdateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await _questionRepository.Query
                .Include(q => q.Answers)
                .Filter(q => q.Id == id)
                .GetSingleAsync();

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            await _questionRepository.DeleteCommitAsync(entity);

            return ErrorCode.Success;
        }
    }
}
