using ESchool.Admin.ViewModels.Messages;
using FluentValidation;

namespace ESchool.Admin.Validators.Messages
{
    public class EmailAccountValidator : AbstractValidator<EmailAccountViewModel>
    {
        public EmailAccountValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty();

            RuleFor(p => p.Host)
                .NotEmpty();

            RuleFor(p => p.Port)
                .InclusiveBetween(ushort.MinValue + 1, ushort.MaxValue);

            RuleFor(p => p.UserName)
                .NotEmpty();

            RuleFor(p => p.Password)
                .NotEmpty();
        }
    }
}
