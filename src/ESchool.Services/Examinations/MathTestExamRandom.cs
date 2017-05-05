using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class MathTestExamRandom
    {
        private readonly string Part1QTagName = "Kỹ năng tính toán cơ bản";
        private readonly string Part2QTagName = "Kiến thức cũ";
        private readonly string Part3QTagName = "Kiến thức hiện tại";
        private readonly string Part4QTagName = "Kiến thức trước chương trình";
        private readonly string Part5QTagName = "Nâng cao";
        private readonly string Child1QTagName = "Đại số";
        private readonly string Child2QTagName = "Hình học";

        private readonly ObjectDbContext _dbContext;
        private readonly IQTagService _qtagService;

        private readonly int _groupId;
        private readonly bool _specialized;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly int[] _exceptList;

        public MathTestExamRandom(
            ObjectDbContext dbContext,
            IQTagService qtagService,
            int groupId,
            bool specialized,
            DateTime fromDate,
            DateTime toDate,
            int[] exceptList)
        {
            _dbContext = dbContext;
            _qtagService = qtagService;

            _groupId = groupId;
            _specialized = specialized;
            _fromDate = fromDate;
            _toDate = toDate;
            _exceptList = exceptList;
        }

        public void DoRandom(int qtagId, int totalQuestion, float totalGrade)
        {
        }

        public async Task DoRandom(int groupId, DateTime month)
        {
            var hierarchicalQTags = await _qtagService.GetListAsync(groupId);

            foreach (var qtag in hierarchicalQTags)
            {
                if (qtag.Name.Equals(Part1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    RandomPart1(qtag.SubQTags);
                }
                else if (qtag.Name.Equals(Part2QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    RandomPart2(qtag.SubQTags);
                }
                else if (qtag.Name.Equals(Part3QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    RandomPart3(qtag.SubQTags);
                }
                else if (qtag.Name.Equals(Part4QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    RandomPart4(qtag.SubQTags);
                }
                else if (qtag.Name.Equals(Part5QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    RandomPart5(qtag.SubQTags);
                }
            }
        }

        private void RandomPart1(IList<QTagDto> qtags)
        {
            foreach (var qtag in qtags)
            {
                if (qtag.Name.Equals(Child1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 2,
                        TotalGrade = 1,
                        SubQTags = qtag.SubQTags
                    };

                    break;
                }
            }
        }

        private void RandomPart2(IList<QTagDto> qtags)
        {
            foreach (var qtag in qtags)
            {
                if (qtag.Name.Equals(Child1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 2,
                        TotalGrade = 1,
                        SubQTags = qtag.SubQTags
                    };
                }
                else if (qtag.Name.Equals(Child2QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 2,
                        TotalGrade = 1,
                        SubQTags = qtag.SubQTags
                    };
                }
            }
        }

        private void RandomPart3(IList<QTagDto> qtags)
        {
            foreach (var qtag in qtags)
            {
                if (qtag.Name.Equals(Child1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 6,
                        TotalGrade = 3,
                        SubQTags = qtag.SubQTags
                    };
                }
                else if (qtag.Name.Equals(Child2QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 4,
                        TotalGrade = 2,
                        SubQTags = qtag.SubQTags
                    };
                }
            }
        }

        private void RandomPart4(IList<QTagDto> qtags)
        {
            foreach (var qtag in qtags)
            {
                if (qtag.Name.Equals(Child1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 1,
                        TotalGrade = 0.5F,
                        SubQTags = qtag.SubQTags
                    };
                }
                else if (qtag.Name.Equals(Child2QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 1,
                        TotalGrade = 0.5F,
                        SubQTags = qtag.SubQTags
                    };
                }
            }
        }

        private void RandomPart5(IList<QTagDto> qtags)
        {
            foreach (var qtag in qtags)
            {
                if (qtag.Name.Equals(Child1QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 1,
                        TotalGrade = 0.5F,
                        SubQTags = qtag.SubQTags
                    };
                }
                else if (qtag.Name.Equals(Child2QTagName, StringComparison.OrdinalIgnoreCase))
                {
                    var testExamScaffold = new TestExamScaffold
                    {
                        QTagId = qtag.Id,
                        TotalQuestion = 1,
                        TotalGrade = 0.5F,
                        SubQTags = qtag.SubQTags
                    };
                }
            }
        }

        private async Task<IList<QuestionExamPaper>> RandomTestExamScaffold(TestExamScaffold testExamScaffold)
        {
            var qtagIds = testExamScaffold.SubQTags.Select(t => t.Id).ToList();

            var questions = await Questions.AsNoTracking()
                    .Where(q => qtagIds.Contains(q.QTagId))
                    .Select(q => new { QTagId = q.QTagId, Id = q.Id })
                    .ToListAsync();

            var random = new Random();
            IList<QuestionExamPaper> questionIds = new List<QuestionExamPaper>();
            float gradePerQuestion = testExamScaffold.TotalGrade / testExamScaffold.TotalQuestion;

            do
            {
                var question = questions[random.Next(questions.Count)];

                questionIds.Add(new QuestionExamPaper
                {
                    QuestionId = question.Id,
                    Grade = gradePerQuestion
                });

                questions.RemoveAll(q => q.QTagId == question.QTagId);
            } while (questionIds.Count < testExamScaffold.TotalQuestion);

            return questionIds;
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

    public class TestExamScaffold
    {
        public int QTagId { get; set; }

        public int TotalQuestion { get; set; }

        public float TotalGrade { get; set; }

        public IList<QTagDto> SubQTags { get; set; }
    }
}
