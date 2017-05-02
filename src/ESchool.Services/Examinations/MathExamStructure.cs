using System;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class MathExamStructure
    {
        private readonly string[] GroupNames = new string[]
        {
            "Kỹ năng tính toán cơ bản",
            "Kiến thức cũ",
            "Kiến thức hiện tại",
            "Kiến thức trước chương trình",
            "Nâng cao"
         };

        private readonly ObjectDbContext _dbContext;

        public MathExamStructure(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DoRandom(int groupId, DateTime month)
        {
            var qtags = await QTags.AsNoTracking()
                .Where(g => GroupNames.Contains(g.Name))
                .ToListAsync();

            var qtagIds = qtags.Select(t => t.Id);

            var questions = Questions.AsNoTracking()
                .Include(q => q.QTag)
                .Where(q => q.QTag.GroupId == groupId && q.Month <= month && (qtagIds.Contains(q.QTagId) || qtagIds.Contains(q.QTag.ParentId)))
                .Select(q => new
                {
                    QTagId = q.QTagId,
                    QuestionId = q.Id,
                    Specialized = q.Specialized
                });


            foreach (var qtag in qtags)
            {
                
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
    }
}
