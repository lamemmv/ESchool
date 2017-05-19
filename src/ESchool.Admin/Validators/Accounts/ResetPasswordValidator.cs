using ESchool.Admin.ViewModels.Accounts;
using FluentValidation;

namespace ESchool.Admin.Validators.Accounts
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.Password);

            RuleFor(p => p.Code)
                .NotEmpty();
        }
    }
}
