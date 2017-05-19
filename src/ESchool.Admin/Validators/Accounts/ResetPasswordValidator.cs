using ESchool.Admin.ViewModels.Accounts;
using ESchool.Services.Constants;
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
                .NotEmpty()
                .Must(p => p.Length >= ValidationRules.MinPasswordLength);

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.Password);

            RuleFor(p => p.Code)
                .NotEmpty();
        }
    }
}
