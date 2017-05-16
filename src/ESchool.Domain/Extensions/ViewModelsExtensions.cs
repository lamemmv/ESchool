using System.Collections.Generic;
using System.Linq;
using ESchool.Domain.Entities.Accounts;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Entities.Messages;
using ESchool.Domain.ViewModels.Accounts;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Domain.ViewModels.Messages;

namespace ESchool.Domain.Extensions
{
    public static class ViewModelsExtensions
    {
        #region Messages

        public static EmailAccount ToEmailAccount(this EmailAccountViewModel viewModel, int id = 0)
        {
            return new EmailAccount
            {
                Id = id,
                Email = viewModel.Email.Trim(),
                DisplayName = viewModel.DisplayName.TrimNull(),
                Host = viewModel.Host.Trim(),
                Port = viewModel.Port,
                UserName = viewModel.UserName.Trim(),
                Password = viewModel.Password.Trim(),
                EnableSsl = viewModel.EnableSsl,
                UseDefaultCredentials = viewModel.UseDefaultCredentials,
                IsDefaultEmailAccount = viewModel.IsDefaultEmailAccount
            };
        }

        #endregion

        #region Accounts

        public static ApplicationUser ToApplicationUser(this AccountViewModel viewModel, string id = null)
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = viewModel.UserName.Trim(),
                Email = viewModel.Email.Trim()
            };
        }

        #endregion

        #region Examinations

        public static QTag ToQTag(this QTagViewModel viewModel, int id = 0)
        {
            return new QTag
            {
                GroupId = viewModel.GroupId,
                ParentId = viewModel.ParentId,
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
                Specialized = viewModel.Specialized,
                Month = viewModel.Month,
                QTagId = viewModel.QTagId,
                Answers = answers
            };
        }

        public static ExamPaper ToExamPaper(this ExamPaperViewModel viewModel, int id = 0)
        {
            return new ExamPaper
            {
                Id = id,
                Name = viewModel.Name.Trim(),
                GroupId = viewModel.GroupId,
                Duration = viewModel.Duration,
                Specialized = viewModel.Specialized
            };
        }

        #endregion
    }
}
