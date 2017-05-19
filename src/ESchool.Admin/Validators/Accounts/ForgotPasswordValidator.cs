using ESchool.Admin.ViewModels.Accounts;
using FluentValidation;

namespace ESchool.Admin.Validators.Accounts
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Url)
                .NotEmpty();
        }
    }
}
