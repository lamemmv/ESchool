using System.Collections.Generic;
using System.Linq;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.ViewModels.Examinations;

namespace ESchool.Domain.Extensions
{
    public static class ViewModelsExtensions
    {
        public static QTag ToQTag(this QTagViewModel viewModel, int id = 0)
        {
            return new QTag
            {
                ParentId = viewModel.ParentId,
                GroupQTags = new List<GroupQTag>
                {
                    new GroupQTag { GroupId = viewModel.GroupId }
                },
                Id = id,
                Name = viewModel.Name.Trim(),
                Description = viewModel.Description.TrimNull()
            };
        }

        public static Question ToQuestion(this QuestionViewModel viewModel, int id = 0)
        {
            IList<Answer> answers = null;

            if (viewModel.Answers != null && viewModel.Answers.Length > 0)
            {
                answers = viewModel.Answers.Select(a => new Answer
                {
                    AnswerName = a.AnswerName.Trim(),
                    Body = a.Body.Trim(),
                    DSS = a.DSS
                }).ToList();
            }

            return new Question
            {
                Id = id,
                Content = viewModel.Content.Trim(),
                Description = viewModel.Description.TrimNull(),
                Type = viewModel.Type,
                DifficultLevel = viewModel.DifficultLevel,
                Answers = answers
            };
        }

        public static ExamPaper ToExamPaper(this ExamPaperViewModel viewModel, int id = 0)
        {
            return new ExamPaper
            {
                Id = id,
                Name = viewModel.Name.Trim()
            };
        }
    }
}
