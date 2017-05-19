using ESchool.Admin.ViewModels.Systems;
using FluentValidation;

namespace ESchool.Admin.Validators.Accounts
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty();

            RuleFor(p => p.Password)
                .NotEmpty();
        }
    }
}
