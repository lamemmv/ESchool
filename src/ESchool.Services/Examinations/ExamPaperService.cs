using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class ExamPaperService : BaseService, IExamPaperService
    {
        public ExamPaperService(ObjectDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<ExamPaperDto> GetAsync(int id)
        {
            var entity = await ExamPapers.AsNoTracking()
                .Include(ep => ep.QuestionExamPapers)
                    .ThenInclude(qep => qep.Question)
                .SingleOrDefaultAsync(ep => ep.Id == id);

            if (entity == null)
            {
                return null;
            }

            return entity.ToExamPaperDto();
        }

        public async Task<IPagedList<ExamPaperDto>> GetListAsync(int page, int size)
        {
            return await ExamPapers.AsNoTracking()
                .Include(ep => ep.QuestionExamPapers)
                    .ThenInclude(qep => qep.Question)
                .OrderBy(ep => ep.Id)
                .Select(ep => ep.ToExamPaperDto())
                .GetListAsync(page, size);
        }

        public async Task<ErrorCode> CreateAsync(string examPaperName, int[] questionIds)
        {
            if (questionIds != null && questionIds.Length > 0)
            {
                entity.QuestionExamPapers = questionIds.Select(q => new QuestionExamPaper { QuestionId = q }).ToList();
            }

            await ExamPapers.AddAsync(entity);

            return await CommitAsync();
        }

        public async Task<ErrorCode> UpdateAsync(ExamPaper entity, int[] questionIds)
        {
            var updatedEntity = await ExamPapers.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                return ErrorCode.NotFound;
            }

            // Delete current QuestionExamPapers.
            DeleteQuestionExamPapers(entity.Id);

            // Update.
            updatedEntity.Name = entity.Name;

            if (questionIds != null && questionIds.Length > 0)
            {
                entity.QuestionExamPapers = questionIds.Select(q => new QuestionExamPaper { QuestionId = q }).ToList();
            }

            return await CommitAsync();
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var dbSet = ExamPapers;
            var entity = await dbSet
                .Include(ep => ep.QuestionExamPapers)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            dbSet.Remove(entity);

            return await CommitAsync();
        }

        private DbSet<ExamPaper> ExamPapers
        {
            get
            {
                return _dbContext.Set<ExamPaper>();
            }
        }

        private DbSet<Question> Questions
        {
            get
            {
                return _dbContext.Set<Question>();
            }
        }

        private void DeleteQuestionExamPapers(int examPaperId)
        {
            var dbSet = _dbContext.Set<QuestionExamPaper>();
            var questionExamPapers = dbSet.Where(qep => qep.ExamPaperId == examPaperId);

            if (questionExamPapers.Any())
            {
                dbSet.RemoveRange(questionExamPapers);
            }
        }
    }
}
