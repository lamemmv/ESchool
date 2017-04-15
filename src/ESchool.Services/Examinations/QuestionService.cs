using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESchool.Services.Examinations
{
    public class QuestionService : BaseService, IQuestionService
    {
        public QuestionService(ObjectDbContext dbContext, ILogger<QuestionService> logger)
            : base(dbContext, logger)
        {
        }

        public async Task<Question> FindAsync(int id)
        {
            return await Questions.AsNoTracking()
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IPagedList<QuestionDto>> GetListAsync(int page, int size)
        {
            var questions = Questions.AsNoTracking()
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .OrderBy(q => q.Id)
                .Select(q => q.ToQuestionDto());

            return await questions.GetListAsync(page, size);
        }

        public async Task<ErrorCode> CreateAsync(Question entity, string[] qtags)
        {
            if (qtags != null && qtags.Length > 0)
            {
                qtags = await AddQTags(qtags);

                entity.QuestionTags = qtags.Select(t => new QuestionTag { QTag = new QTag { Name = t } }).ToList();
            }

            await Questions.AddAsync(entity);

            return await CommitAsync();
        }

        public async Task<ErrorCode> UpdateAsync(Question entity, string[] qtags)
        {
            var updatedEntity = await Questions.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                return ErrorCode.NotFound;
            }

            // Delete current QuestionTags & Answers.
            DeleteQuestionTags(entity.Id);
            DeleteAnswers(entity.Id);

            // Update.
            updatedEntity.Content = entity.Content;
            updatedEntity.Description = entity.Description;
            updatedEntity.Type = entity.Type;

            if (qtags != null && qtags.Length > 0)
            {
                qtags = await AddQTags(qtags);

                entity.QuestionTags = qtags.Select(t => new QuestionTag { QTag = new QTag { Name = t } }).ToList();
            }

            updatedEntity.Answers = entity.Answers;

            return await CommitAsync();
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var dbSet = Questions;
            var entity = await dbSet
                .Include(q => q.QuestionTags)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
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


        private void DeleteQuestionTags(int questionId)
        {
            var dbSet = _dbContext.Set<QuestionTag>();
            var questionTags = dbSet.Where(qt => qt.QuestionId == questionId);

            if (questionTags.Any())
            {
                dbSet.RemoveRange(questionTags);
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

        private async Task<string[]> AddQTags(string[] qtags)
        {
            qtags = qtags.Distinct().Select(t => t.Trim()).ToArray();

            var dbSet = _dbContext.Set<QTag>();

            var existingQTags = dbSet.Where(t => qtags.Contains(t.Name));

            var newQTags = qtags.Except(existingQTags.Select(t => t.Name));

            if (newQTags.Any())
            {
                await dbSet.AddRangeAsync(newQTags.Select(t => new QTag { Name = t }));

                await _dbContext.SaveChangesAsync();
            }

            return qtags;
        }
    }
}
