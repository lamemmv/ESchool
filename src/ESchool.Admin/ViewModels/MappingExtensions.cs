using System.Collections.Generic;
using System.Linq;
using ESchool.Admin.ViewModels.Accounts;
using ESchool.Admin.ViewModels.Examinations;
using ESchool.Admin.ViewModels.Messages;
using ESchool.Data.Entities.Accounts;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Entities.Messages;
using ESchool.Services.Infrastructure.Extensions;

namespace ESchool.Admin.ViewModels
{
    public static class MappingExtensions
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

        public static ApplicationUser ToApplicationUser(this CreateAccountViewModel viewModel, string id = null)
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
