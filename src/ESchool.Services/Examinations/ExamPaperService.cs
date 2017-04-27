using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Services.Exceptions;
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

        public async Task<ExamPaper> CreateAsync(ExamPaper entity, IList<int> questionIds)
        {
            if (questionIds != null && questionIds.Count > 0)
            {
                entity.QuestionExamPapers = questionIds.Select(q => new QuestionExamPaper { QuestionId = q }).ToList();
            }

            await ExamPapers.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(ExamPaper entity, IList<int> questionIds)
        {
            var updatedEntity = await ExamPapers.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("ExamPaper not found.");
            }

            // Delete current QuestionExamPapers.
            DeleteQuestionExamPapers(entity.Id);

            // Update.
            updatedEntity.Name = entity.Name;

            if (questionIds != null && questionIds.Count > 0)
            {
                entity.QuestionExamPapers = questionIds.Select(q => new QuestionExamPaper { QuestionId = q }).ToList();
            }

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var dbSet = ExamPapers;
            var entity = await dbSet
                .Include(ep => ep.QuestionExamPapers)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException("ExamPaper not found.");
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

        private DbSet<QuestionExamPaper> QuestionExamPapers
        {
            get
            {
                return _dbContext.Set<QuestionExamPaper>();
            }
        }

        private void DeleteQuestionExamPapers(int examPaperId)
        {
            var dbSet = QuestionExamPapers;
            var questionExamPapers = dbSet.Where(qep => qep.ExamPaperId == examPaperId);

            if (questionExamPapers.Any())
            {
                dbSet.RemoveRange(questionExamPapers);
            }
        }
    }
}
