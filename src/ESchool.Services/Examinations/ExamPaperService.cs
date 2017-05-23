using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;
using ESchool.Services.Exceptions;
using ESchool.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class ExamPaperService : BaseService, IExamPaperService
    {
        public ExamPaperService(ObjectDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IList<QuestionExamPaper>> GetRandomQuestionsAsync(
            int qtagId,
            bool specialized,
            DateTime fromDate,
            DateTime toDate,
            IList<int> exceptList,
            float totalGrade,
            int totalQuestion)
        {
            float grade = totalGrade / totalQuestion;
            DateTime startDate = fromDate.StartOfDay();
            DateTime endDate = toDate.EndOfDay();

            var query = Questions.AsNoTracking()
                .Where(q => q.QTagId == qtagId &&
                    q.Specialized == specialized &&
                    q.Month >= startDate && q.Month <= endDate);

            if (exceptList != null && exceptList.Count > 0)
            {
                query = query.Where(q => !exceptList.Contains(q.Id));
            }

            if (await query.CountAsync() < totalQuestion)
            {
                string parameterString = RandomParametersToString(
                    qtagId,
                    specialized,
                    fromDate,
                    toDate,
                    exceptList,
                    totalGrade,
                    totalQuestion);

                throw new RandomExamPaperException($"[{nameof(ExamPaperService)}]: Not enough Questions for random. Parameters: {parameterString}");
            }

            return await query
                .OrderBy(q => Guid.NewGuid())
                .Take(totalQuestion)
                .Select(q => new QuestionExamPaper
                {
                    Grade = grade,
                    QuestionId = q.Id
                })
                .ToListAsync();
        }

        public async Task<ExamPaper> GetAsync(int id)
        {
            return await ExamPapers.AsNoTracking()
                .Include(ep => ep.QuestionExamPapers)
                    .ThenInclude(qep => qep.Question)
                .SingleOrDefaultAsync(ep => ep.Id == id);
        }

        public async Task<IPagedList<ExamPaper>> GetListAsync(int page, int size)
        {
            return await ExamPapers.AsNoTracking()
                .Include(ep => ep.QuestionExamPapers)
                    .ThenInclude(qep => qep.Question)
                .OrderBy(ep => ep.GroupId)
                .GetListAsync(page, size);
        }

        public async Task<ExamPaper> CreateAsync(ExamPaper entity)
        {
            await ExamPapers.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var dbSet = ExamPapers;
            ExamPaper entity = await dbSet
                .Include(ep => ep.QuestionExamPapers)
                .SingleOrDefaultAsync(ep => ep.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(id, nameof(ExamPaper));
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

        private string RandomParametersToString(
            int qtagId,
            bool specialized,
            DateTime fromDate,
            DateTime toDate,
            IList<int> exceptList,
            float totalGrade,
            int totalQuestion)
        {
            return string.Format(
                "[QTagId] = {0}, [Specialized] = {1}, [FromDate] = {2}, [ToDate] = {3}, [ExceptList] = {4}, [TotalGrade] = {5}, [TotalQuestion] = {6}",
                qtagId,
                specialized,
                fromDate,
                toDate,
                string.Join(",", exceptList ?? new List<int>()),
                totalGrade,
                totalQuestion);
        }
    }
}
