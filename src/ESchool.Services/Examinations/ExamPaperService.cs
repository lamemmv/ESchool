using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;
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
            var entity = await dbSet
                .Include(ep => ep.QuestionExamPapers)
                .SingleOrDefaultAsync(ep => ep.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException("ExamPaper not found.");
            }

            dbSet.Remove(entity);

            return await CommitAsync();
        }

        //public async Task AAA(QuestionExamPaperViewModel[] parts)
        //{
        //    var parentQTagIds = parts.Select(p => p.QTagId).ToList();

        //    var qtagIds = await QTags.AsNoTracking()
        //        .Where(t => parentQTagIds.Contains(t.ParentId))
        //        .Select(t => t.Id)
        //        .ToListAsync();

        //    var questions = Questions.AsNoTracking()
        //        .Include(q => q.QTag)
        //        .Where(q => qtagIds.Contains(q.QTagId))
        //        .GroupBy(q => q.QTag.ParentId);

        //    //var random = new Random();

        //    //foreach (var part in parts)
        //    //{
        //    //    var aaa = qtags[part.QTagId].ToList();

        //    //    do
        //    //    {
        //    //        var index = random.Next(aaa.Count);

        //    //    } while (true);
        //    //}
        //}

        private DbSet<ExamPaper> ExamPapers
        {
            get
            {
                return _dbContext.Set<ExamPaper>();
            }
        }

        private DbSet<QTag> QTags
        {
            get
            {
                return _dbContext.Set<QTag>();
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
