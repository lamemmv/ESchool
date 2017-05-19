using ESchool.Admin.ViewModels.Examinations;
using ESchool.Services.Constants;
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

            RuleFor(p => p.FromDate)
                .InclusiveBetween(ValidationRules.MinDate, ValidationRules.MaxDate)
                .LessThanOrEqualTo(p => p.ToDate);

            RuleFor(p => p.ToDate)
                .InclusiveBetween(ValidationRules.MinDate, ValidationRules.MaxDate);
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
