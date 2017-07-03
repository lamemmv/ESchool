using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.DTOs.Examinations;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;
using ESchool.Services.Enums;
using ESchool.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class QuestionService : BaseService, IQuestionService
    {
        public QuestionService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<QuestionDto> GetAsync(int id)
        {
            Question entity = await Questions.AsNoTracking()
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                return null;
            }

            return ToQuestionDto(entity);
        }

        public async Task<IPagedList<Question>> GetListAsync(int page, int size)
        {
            return await Questions.AsNoTracking()
                .Include(q => q.Answers)
                .OrderBy(q => q.Id)
                .GetListAsync(page, size);
        }

        public async Task<Question> CreateAsync(Question entity)
        {
            await Questions.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(Question entity)
        {
            Question updatedEntity = await Questions.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new ApiException(
                    $"{nameof(Question)} not found. Id = {entity.Id}",
                    ApiErrorCode.NotFound);
            }

            // Delete current QuestionTags & Answers.
            DeleteAnswers(entity.Id);

            // Update.
            updatedEntity.Content = entity.Content;
            updatedEntity.Description = entity.Description;
            updatedEntity.Type = entity.Type;
            updatedEntity.DifficultLevel = entity.DifficultLevel;
            updatedEntity.Specialized = entity.Specialized;
            updatedEntity.Month = entity.Month;
            updatedEntity.QTagId = entity.QTagId;
            updatedEntity.Answers = entity.Answers;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            DbSet<Question> dbSet = Questions;
            Question entity = await dbSet
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                throw new ApiException(
                    $"{nameof(Question)} not found. Id = {id}",
                    ApiErrorCode.NotFound);
            }

            dbSet.Remove(entity);

            return await CommitAsync();
        }

        private DbSet<Question> Questions
        {
            get
            {
                return _dbContext.Set<Question>();
            }
        }

        private void DeleteAnswers(int questionId)
        {
            var dbSet = _dbContext.Set<Answer>();
            var answers = dbSet.Where(a => a.QuestionId == questionId);

            if (answers.Any())
            {
                dbSet.RemoveRange(answers);
            }
        }

        private QuestionDto ToQuestionDto(Question entity)
        {
            return new QuestionDto
            {
                Id = entity.Id,
                Content = entity.Content,
                Description = entity.Description,
                Type = entity.Type,
                DifficultLevel = entity.DifficultLevel,
                Specialized = entity.Specialized,
                Month = entity.Month,
                QTagId = entity.QTagId,
                Answers = entity.Answers
            };
        }
    }
}
