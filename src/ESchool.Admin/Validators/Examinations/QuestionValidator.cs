using ESchool.Admin.ViewModels.Examinations;
using ESchool.Services.Constants;
using FluentValidation;

namespace ESchool.Admin.Validators.Examinations
{
    public class QuestionValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionValidator()
        {
            RuleFor(p => p.Content)
                .NotEmpty();

            RuleFor(p => p.Type)
                .InclusiveBetween(1, 2);

            //RuleFor(p => p.DifficultLevel)
            //    .InclusiveBetween(1, 5);

            RuleFor(p => p.Month)
                .InclusiveBetween(ValidationRules.MinDate, ValidationRules.MaxDate);

            RuleFor(p => p.Type)
                .GreaterThan(0);
        }
    }
}
