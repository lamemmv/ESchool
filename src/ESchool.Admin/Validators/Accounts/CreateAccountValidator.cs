using ESchool.Admin.ViewModels.Accounts;
using FluentValidation;

namespace ESchool.Admin.Validators.Accounts
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountViewModel>
    {
        public CreateAccountValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty();

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Equal(p => p.Password);
        }
    }
}
