using System.Collections.Generic;
using System.Linq;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.ViewModels.Examinations;

namespace ESchool.Domain.Extensions
{
    public static class ViewModelsExtensions
    {
        public static QTag ToQTag(this QTagViewModel viewModel)
        {
            return new QTag
            {
                Name = viewModel.Name.Trim(),
                Description = viewModel.Description.TrimNull()
            };
        }

        public static Question ToQuestion(this QuestionCreateViewModel viewModel)
        {
            IList<QuestionTag> questionTags = null;
            IList<Answer> answers = null;

            if (viewModel.QTagIds != null && viewModel.QTagIds.Length > 0)
            {
                questionTags = viewModel.QTagIds.Select(t => new QuestionTag
                {
                    QTagId = t
                }).ToList();
            }

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
                Content = viewModel.Content.Trim(),
                Description = viewModel.Description.TrimNull(),
                Type = viewModel.Type,
                QuestionTags = questionTags,
                Answers = answers
            };
        }
    }
}
