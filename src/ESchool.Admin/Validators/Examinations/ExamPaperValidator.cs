using System;
using ESchool.Admin.ViewModels.Examinations;
using FluentValidation;

namespace ESchool.Admin.Validators.Examinations
{
    public class ExamPaperValidator : AbstractValidator<ExamPaperViewModel>
    {
        public ExamPaperValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.GroupId)
                .GreaterThan(0);

            RuleFor(p => p.Duration)
                .GreaterThan(0);

            var fromDate = new DateTime(2015, 1, 1);
            var toDate = fromDate.AddYears(100);
            RuleFor(p => p.FromDate)
                .InclusiveBetween(fromDate, toDate)
                .LessThanOrEqualTo(p => p.ToDate);

            RuleFor(p => p.ToDate)
                .InclusiveBetween(fromDate, toDate);
        }
    }

    public class QuestionExamPaperValidator : AbstractValidator<QuestionExamPaperViewModel>
    {
        public QuestionExamPaperValidator()
        {
            RuleFor(p => p.QTagId)
                .GreaterThan(0);

            RuleFor(p => p.TotalQuestion)
                .GreaterThan(0);

            RuleFor(p => p.TotalGrade)
                .GreaterThan(0);
        }
    }
}
