using ESchool.Admin.ViewModels.Accounts;
using FluentValidation;

namespace ESchool.Admin.Validators.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.OldPassword)
                .NotEmpty();

            RuleFor(p => p.NewPassword)
                .NotEmpty();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.NewPassword);
        }
    }
}
