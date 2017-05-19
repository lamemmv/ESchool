using ESchool.Admin.ViewModels.Examinations;
using FluentValidation;

namespace ESchool.Admin.Validators.Examinations
{
    public class QTagValidator : AbstractValidator<QTagViewModel>
    {
        public QTagValidator()
        {
            RuleFor(p => p.GroupId)
                .GreaterThan(0);

            RuleFor(p => p.ParentId)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.Name)
                .NotEmpty();
        }
    }
}
