using ESchool.Admin.ViewModels.Examinations;
using FluentValidation;

namespace ESchool.Admin.Validators.Examinations
{
    public class AnswerValidator : AbstractValidator<AnswerViewModel>
    {
        public AnswerValidator()
        {
            RuleFor(p => p.AnswerName)
                .NotEmpty();

            RuleFor(p => p.Body)
                .NotEmpty();
        }
    }
}
